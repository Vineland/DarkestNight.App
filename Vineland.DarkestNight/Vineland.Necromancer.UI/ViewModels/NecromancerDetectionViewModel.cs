using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.UI;
using XLabs.Ioc;

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
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService, roll: _seer.ProphecyOfDoomRoll));

			if (_wizard != null && _wizard.RuneOfMisdirectionActive)
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService));

			SelectedResult = Results.First ();
		}

		public List<NecromancerDetectionResultViewModel> Results { get; set; }

		public NecromancerDetectionResultViewModel SelectedResult { get; set; }


		public RelayCommand SpawnCommand {
			get {
				return new RelayCommand (() => {
					
					var viewModel = Resolver.Resolve<NecromancerSpawnViewModel> ();
					viewModel.Initialise (SelectedResult.Result, SelectedResult.PendingGameState.Clone());

					Application.Navigation.Push<NecromancerSpawnPage> (viewModel);
				});
			}
		}
	}

	public class NecromancerDetectionResultViewModel : BaseViewModel
	{

		NecromancerDetectionResult _result;
		NecromancerService _necromancerService;

		public GameState PendingGameState { get; private set; }

		Acolyte _acolyte;
		Wayfarer _wayfarer;
		Valkyrie _valkyrie;
		Shaman _shaman;

		public NecromancerDetectionResultViewModel (NecromancerService necroService, int? roll = null)
		{
			_necromancerService = necroService;

			ActivateNecromancer (roll: roll);

			_acolyte = Application.CurrentGame.Heroes.GetHero<Acolyte> ();
			_wayfarer = Application.CurrentGame.Heroes.GetHero <Wayfarer> ();
			_valkyrie = Application.CurrentGame.Heroes.GetHero <Valkyrie> ();
			_shaman = Application.CurrentGame.Heroes.GetHero<Shaman> ();
		}

		private void ActivateNecromancer (Hero detectedHero = null,
		                                  int? roll = null, 
		                                  int[] heroesToIgnore = null)
		{
			PendingGameState = Application.CurrentGame.Clone ();
			Result = _necromancerService.Activate (PendingGameState, detectedHero, roll, heroesToIgnore);
		}

		public NecromancerDetectionResult Result {
			get{ return _result; }
			set {
				_result = value;
				RaisePropertyChanged (() => Result);
				RaisePropertyChanged (() => NewLocation);
				RaisePropertyChanged (() => BlindingBlackVisible);
				RaisePropertyChanged (() => DecoyVisible);
				RaisePropertyChanged (() => VoidArmorVisible);
				RaisePropertyChanged (() => ElusiveSpiritVisible);
			}
		}

		public Location NewLocation
		{
			get{ return PendingGameState.Locations.SingleOrDefault (l => l.Id == Result.NewLocationId); }
		}

		public string Notes{
			get { return  Result.Notes; }
		}

		List<int> _heroesToIgnore = new List<int> ();

		public bool VoidArmorVisible {
			get{ return Result.DetectedHero != null && Result.DetectedHero.HasVoidArmor; }
		}

		public RelayCommand VoidArmorCommand {
			get {
				return new RelayCommand (() => {
					_heroesToIgnore.Add (Result.DetectedHero.Id);
					ActivateNecromancer (roll: Result.NecromancerRoll, heroesToIgnore: _heroesToIgnore.ToArray ());
				},
					() => {
						return Result.DetectedHero != null;
					});
			}
		}

		public bool BlindingBlackVisible {
			get {
				return _acolyte != null
				&& _acolyte.BlindingBlackActive
				&& Result.DetectedHero != null;
			}
			
		}

		public RelayCommand BlindingBlackCommand {
			get {
				return new RelayCommand (() => {
					_heroesToIgnore = PendingGameState.Heroes.Select (x => x.Id).ToList();
					_acolyte.BlindingBlackActive = false;
					ActivateNecromancer (roll: Result.NecromancerRoll, heroesToIgnore: _heroesToIgnore.ToArray ());
				});
			}
		}

		public bool DecoyVisible {
			get {
				return _wayfarer != null
				&& _wayfarer.DecoyActive
				&& Result.DetectedHero != null
					&& _wayfarer.LocationId != (int)LocationIds.Monastery
				&& _wayfarer.Secrecy >= Result.DetectionRoll;
			}
		}

		public RelayCommand DecoyCommand {
			get {
				return new RelayCommand (() => {
					ActivateNecromancer (detectedHero: _wayfarer, roll: Result.NecromancerRoll, heroesToIgnore: _heroesToIgnore.ToArray ());
				});
			}
		}

		public bool ElusiveSpiritVisible {
			get {
				return _valkyrie != null
				&& _valkyrie.ElusiveSpiritActive
				&& Result.DetectedHero != null
				&& Result.DetectionRoll == Result.DetectedHero.Secrecy + 1;
			}
		}

		public RelayCommand ElusiveSpiritCommand {
			get {
				return new RelayCommand (() => {
					//TODO could be triggered more than once
					_heroesToIgnore.Add (Result.DetectedHero.Id);
					ActivateNecromancer(roll:Result.NecromancerRoll, heroesToIgnore: _heroesToIgnore.ToArray ());
				});
			}
		}
	}
}

