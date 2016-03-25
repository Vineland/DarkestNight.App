using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.UI;
using XLabs.Ioc;
using Vineland.Necromancer.Core.Services;
using System.Security.Cryptography;
using System.Net;
using Android.App;

namespace Vineland.Necromancer.UI
{
	public class NecromancerDetectionViewModel :BaseViewModel
	{
		NecromancerService _necromancerService;

		#region Notable Heroes

		Seer _seer;
		Wizard _wizard;

		#endregion

		public NecromancerDetectionViewModel (NecromancerService necromancerService)
		{
			_necromancerService = necromancerService;

			_seer = Application.CurrentGame.Heroes.GetHero<Seer> ();
			_wizard = Application.CurrentGame.Heroes.GetHero<Wizard> ();

			Results = new List<NecromancerDetectionResultViewModel> ();

			if (_seer == null || _seer.ProphecyOfDoomRoll == 0)
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService));
			else
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService, _seer.ProphecyOfDoomRoll));

			if (_wizard != null && _wizard.RuneOfMisdirectionActive)
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService));

			SelectedResult = Results.First ();
		}

		public List<NecromancerDetectionResultViewModel> Results { get; set; }

		public NecromancerDetectionResultViewModel SelectedResult { get; set; }

		public bool SkipPage {
			get {
				return Results.Count == 1 && !Results.Any (r => r.DetectedHero == null || r.BlindingBlackVisible || r.DecoyVisible || r.ElusiveSpiritVisible || r.VoidArmorVisible);
			}
		}

		public RelayCommand AcceptCommand {
			get {
				return new RelayCommand (async () => {
					var page = await Application.Navigation.Push<NecromancerActivationPage> ();
					(page.BindingContext as NecromancerActivationViewModel).Initialise (SelectedResult.DetectedHero, SelectedResult.NecromancerRoll, SelectedResult.HeroesToIgnore.ToArray());
				});
			}
		}
	}

	public class NecromancerDetectionResultViewModel : BaseViewModel
	{
		NecromancerService _necromancerService;

		Acolyte _acolyte;
		Wayfarer _wayfarer;
		Valkyrie _valkyrie;
		Shaman _shaman;

		public NecromancerDetectionResultViewModel (NecromancerService necromancerService, int? roll = null)
		{
			_necromancerService = necromancerService;
			if (!roll.HasValue)
				roll = new D6GeneratorService ().RollDemBones ();

			_acolyte = Application.CurrentGame.Heroes.GetHero<Acolyte> ();
			_wayfarer = Application.CurrentGame.Heroes.GetHero <Wayfarer> ();
			_valkyrie = Application.CurrentGame.Heroes.GetHero <Valkyrie> ();
			_shaman = Application.CurrentGame.Heroes.GetHero<Shaman> ();

			NecromancerRoll = roll.Value;
			DetectHero ();
		}

		public int NecromancerRoll { get; set; }

		private int DetectionRoll {
			get { return NecromancerRoll + (Application.CurrentGame.GatesActive ? 1 : 0); }
		}

		public Hero DetectedHero { get; set; }

		public string NecromancerRollDisplay {
			get { 
				var display = NecromancerRoll.ToString ();
				if (Application.CurrentGame.GatesActive)
					display += " (+1 for detection)";

				return display;
			}
		}

		public List<int> HeroesToIgnore { get; set; } = new List<int> ();

		public bool VoidArmorVisible {
			get{ return DetectedHero?.HasVoidArmor ?? false; }
		}

		public RelayCommand VoidArmorCommand {
			get {
				return new RelayCommand (() => {
					HeroesToIgnore.Add (DetectedHero.Id);
					DetectHero ();
				},
					() => {
						return DetectedHero != null;
					});
			}
		}

		public bool BlindingBlackVisible {
			get {
				return _acolyte?.BlindingBlackActive ?? false;
			}
		}

		public RelayCommand BlindingBlackCommand {
			get {
				return new RelayCommand (() => {
					HeroesToIgnore = Application.CurrentGame.Heroes.Select (x => x.Id).ToList ();
					_acolyte.BlindingBlackActive = false;
					DetectHero ();
				},
					() => {
						return DetectedHero != null
							&& _acolyte != null
						&& _acolyte.BlindingBlackActive;
					});
			}
		}

		public bool DecoyVisible {
			get {
				return _wayfarer?.DecoyActive ?? false;
			}
		}

		public RelayCommand DecoyCommand {
			get {
				return new RelayCommand (() => {
					DetectedHero = _wayfarer;
					RaisePropertyChanged (() => DetectedHero);
					RaisePropertyChanged (() => DecoyCommand);
					RaisePropertyChanged (() => VoidArmorCommand);
				},
					() => {
						return DetectedHero != null
						&& _wayfarer != null
						&& _wayfarer.LocationId != (int)LocationIds.Monastery
						&& _wayfarer.Secrecy >= DetectionRoll;
					});
			}
		}

		public bool ElusiveSpiritVisible {
			get {
				return _valkyrie?.ElusiveSpiritActive ?? false;
			}
		}

		public RelayCommand ElusiveSpiritCommand {
			get {
				return new RelayCommand (() => {
					HeroesToIgnore.Add (DetectedHero.Id);
					DetectHero ();
				},
					() => {
						return DetectedHero != null
						&& DetectionRoll == DetectedHero.Secrecy + 1;
					});
			}
		}

		public void DetectHero ()
		{
			DetectedHero = _necromancerService.Detect (Application.CurrentGame, NecromancerRoll, HeroesToIgnore.ToArray ());
			RaisePropertyChanged (() => DetectedHero);
			RaisePropertyChanged (() => ElusiveSpiritCommand);
			RaisePropertyChanged (() => BlindingBlackCommand);
			RaisePropertyChanged (() => DecoyCommand);
			RaisePropertyChanged (() => VoidArmorCommand);
			//RaisePropertyChanged (() => BlindingBlackCommand);
		}
	}
}

