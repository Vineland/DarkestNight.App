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

namespace Vineland.Necromancer.UI
{
	public class NecromancerDetectionViewModel :BaseViewModel
	{
		NecromancerService _necromancerService;

		public NecromancerDetectionViewModel (NecromancerService necromancerService)
		{
			_necromancerService = necromancerService;

			Results = new List<NecromancerDetectionResultViewModel> ();

			if (Application.CurrentGame.Heroes.ProphecyOfDoomRoll == 0)
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService));
			else
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService, Application.CurrentGame.Heroes.ProphecyOfDoomRoll));

			if (Application.CurrentGame.Heroes.RuneOfMisdirectionActive)
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService));

			SelectedResult = Results.First ();
		}

		public List<NecromancerDetectionResultViewModel> Results { get; set; }

		public NecromancerDetectionResultViewModel SelectedResult { get; set; }

		public bool SkipPage {
			get {
				return Results.Count == 1 && !Results.Any (r => r.DetectedHero != null && (r.BlindingBlackVisible || r.DecoyVisible || r.ElusiveSpiritVisible || r.VoidArmorVisible));
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

		public NecromancerDetectionResultViewModel (NecromancerService necromancerService, int? roll = null)
		{
			_necromancerService = necromancerService;
			if (!roll.HasValue)
				roll = new D6GeneratorService ().RollDemBones ();

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
			get{ return Application.CurrentGame.Heroes.VoidArmorHeroId.HasValue
				&& Application.CurrentGame.Heroes.VoidArmorHeroId.Value == DetectedHero.Id ; }
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
				return Application.CurrentGame.Heroes.BlindingBlackActive;
			}
		}

		public RelayCommand BlindingBlackCommand {
			get {
				return new RelayCommand (() => {
					HeroesToIgnore = Application.CurrentGame.Heroes.Active.Select (x => x.Id).ToList ();
					//_acolyte.BlindingBlackActive = false;
					DetectHero ();
				},
					() => {
						return DetectedHero != null
							&& Application.CurrentGame.Heroes.BlindingBlackActive;
					});
			}
		}

		public bool DecoyVisible {
			get {
				return Application.CurrentGame.Heroes.DecoyActive;
			}
		}

		public RelayCommand DecoyCommand {
			get {
				return new RelayCommand (() => {
					DetectedHero = Application.CurrentGame.Heroes.Active.GetHero<Wayfarer>();
					RaisePropertyChanged (() => DetectedHero);
					RaisePropertyChanged (() => DecoyCommand);
					RaisePropertyChanged (() => VoidArmorCommand);
				},
					() => {
						var wayfarer = Application.CurrentGame.Heroes.Active.GetHero<Wayfarer>();
						return DetectedHero != null
							&& wayfarer.LocationId != (int)LocationIds.Monastery
							&& wayfarer.Secrecy >= DetectionRoll;
					});
			}
		}

		public bool ElusiveSpiritVisible {
			get {
				return Application.CurrentGame.Heroes.ElusiveSpiritActive;
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
		}
	}
}

