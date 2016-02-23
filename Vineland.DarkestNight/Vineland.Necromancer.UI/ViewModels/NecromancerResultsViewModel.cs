using System;
using System.Linq;
using Vineland.Necromancer.Core;
using System.Runtime.InteropServices;
using Android.Util;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using Android.Views.InputMethods;
using Vineland.Necromancer.UI;
using System.Collections.ObjectModel;
using Android.App;

namespace Vineland.Necromancer.UI
{
	public class NecromancerResultsViewModel :BaseViewModel
	{
		NecromancerService _necromancerService;
		NavigationService _navigationService;
		GameStateService _gameStateService;

		public NecromancerResultsViewModel (GameStateService gameStateService, 
			NecromancerService necromancerService, 
			NavigationService navigationService)
		{
			_gameStateService = gameStateService;
			_necromancerService = necromancerService;
			_navigationService = navigationService;

			var results = new List<NecromancerActivationResult> ();
			if (_gameStateService.CurrentGame.Heroes.ProphecyOfDoomRoll == 0)
				results.Add (_necromancerService.Activate (_gameStateService.CurrentGame));
			else
				results.Add (_necromancerService.Activate (_gameStateService.CurrentGame, roll: _gameStateService.CurrentGame.Heroes.ProphecyOfDoomRoll));

			if (_gameStateService.CurrentGame.Heroes.RuneOfMisdirectionActive)
				results.Add (_necromancerService.Activate (_gameStateService.CurrentGame));

			Results = results.Select (x => new NecromancerActivationResultViewModel (x, _gameStateService, _necromancerService)).ToList();
			SelectedResult = Results.First ();
		}

		public List<NecromancerActivationResultViewModel> Results { get; set; }

		public NecromancerActivationResultViewModel SelectedResult { get; set; }


		public RelayCommand Accept {
			get {
				return new RelayCommand (() => {
					
					_gameStateService.CurrentGame.Heroes.ProphecyOfDoomRoll = 0;
					_gameStateService.CurrentGame.Necromancer.LocationId = SelectedResult.Result.NewLocation.Id;
					_gameStateService.CurrentGame.Locations.Single (x => x.Id == SelectedResult.Result.NewLocation.Id).NumberOfBlights += SelectedResult.Result.NumberOfBlightsToNewLocation;
					_gameStateService.CurrentGame.Locations.Single (x => x.Id == LocationIds.Monastery).NumberOfBlights += SelectedResult.Result.NumberOfBlightsToMonastery;
					_gameStateService.Save ();

					_navigationService.PopTo<HeroPhasePage> ();
				});
			}
		}
	}

	public class NecromancerActivationResultViewModel : BaseViewModel
	{

		NecromancerActivationResult _result;
		GameStateService _gameStateService;
		NecromancerService _necromancerService;

		public NecromancerActivationResultViewModel (NecromancerActivationResult result, GameStateService gameStateService, NecromancerService necroService)
		{
			_result = result;
			_gameStateService = gameStateService;
			_necromancerService = necroService;
		}

		public NecromancerActivationResult Result {
			get{ return _result; }
			set {
				_result = value;
				RaisePropertyChanged (() => Result);
			}
		}

		public bool BlindingBlackVisible {
			get {
				return _gameStateService.CurrentGame.Heroes.BlindingBlackActive;
			}
		}

		public RelayCommand BlindingBlackCommand {
			get {
				return new RelayCommand (() => {
					Result = _necromancerService.Activate (_gameStateService.CurrentGame, roll: Result.MovementRoll, heroesToIgnore: _gameStateService.CurrentGame.Heroes.Active.Select (x => x.Id).ToArray ());
				},
					() => {
						return Result.DetectedHero != null;
					});
			}
		}

		public bool DecoyVisible {
			get {
				return _gameStateService.CurrentGame.Heroes.DecoyActive;
			}
		}

		public RelayCommand DecoyCommand {
			get {
				return new RelayCommand (() => {
					var wayfarer = _gameStateService.CurrentGame.Heroes.Active.SingleOrDefault (x => x.Name == "Wayfarer");
					Result = _necromancerService.Activate (_gameStateService.CurrentGame, detectedHero: wayfarer);
				},
					() => {
						var wayfarer = _gameStateService.CurrentGame.Heroes.Active.SingleOrDefault (x => x.Name == "Wayfarer");

						return Result.DetectedHero != null
						&& (wayfarer != null
						&& wayfarer.LocationId != LocationIds.Monastery
						&& wayfarer.Secrecy >= Result.DetectionRoll);
					});
			}
		}

		public bool ElusiveSpiritVisible {
			get { 
				return _gameStateService.CurrentGame.Heroes.ElusiveSpiritActive;
			}
		}

		public RelayCommand ElusiveSpiritCommand {
			get {
				return new RelayCommand (() => {
					//TODO could be triggered more than once
					Result = _necromancerService.Activate (_gameStateService.CurrentGame, heroesToIgnore: new int[] { Result.DetectedHero.Id });
				},
					() => {
						return Result.DetectedHero != null && Result.DetectionRoll == Result.DetectedHero.Secrecy + 1; 
					});
			}
		}
	}
}

