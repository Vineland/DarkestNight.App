using System;
using System.Linq;
using Vineland.Necromancer.Core;
using System.Runtime.InteropServices;
using Android.Util;
using GalaSoft.MvvmLight.Command;
//using Android.Graphics.Drawables;
//using Android.Views.InputMethods;

namespace Vineland.Necromancer.UI
{
	public class NecromancerDetectionViewModel :BaseViewModel
	{
		NecromancerService _necromancerService;
		NavigationService _navigationService;
		GameStateService _gameStateService;

		public NecromancerDetectionViewModel (GameStateService gameStateService, NecromancerService necromancerService, NavigationService navigationService)
		{
			_gameStateService = gameStateService;
			_necromancerService = necromancerService;
			_navigationService = navigationService;

			Result = _necromancerService.Activate (_gameStateService.CurrentGame);
		}
		public NecromancerActivationResult Result { get; set; }


		public bool BlindingBlackAvailable {
			get {
				return Result.DetectedHero != null && _gameStateService.CurrentGame.Heroes.BlindingBlackAttained;
			}
		}

		public RelayCommand BlindingBlackCommand {
			get {
				return new RelayCommand (() => {
					Result = _necromancerService.Activate(_gameStateService.CurrentGame, heroesToIgnore: _gameStateService.CurrentGame.Heroes.Active.Select(x=>x.Id).ToArray());
				});
			}
		}

		public bool DecoyAvailable {
			get {
				var wayfarer = _gameStateService.CurrentGame.Heroes.Active.SingleOrDefault (x => x.Name == "Wayfarer");

				return Result.DetectedHero != null
					&& _gameStateService.CurrentGame.Heroes.DecoyAttained
				&& (wayfarer != null
				&& wayfarer.LocationId != LocationIds.Monastery
				&& wayfarer.Secrecy >= Result.DetectionRoll);
			}
		}

		public RelayCommand DecoyCommand {
			get {
				return new RelayCommand (() => {

					var wayfarer = _gameStateService.CurrentGame.Heroes.Active.SingleOrDefault (x => x.Name == "Wayfarer");
					Result = _necromancerService.Activate(_gameStateService.CurrentGame, detectedHero: wayfarer);
				});
			}
		}

		public bool ElusiveSpiritAvailable {
			get { 
				return Result.DetectedHero != null
					&& _gameStateService.CurrentGame.Heroes.ElusiveSpiritAttained
					&& Result.DetectionRoll == Result.DetectedHero.Secrecy + 1; 
			}
		}

		public RelayCommand ElusiveSpiritCommand {
			get {
				return new RelayCommand (() => {
					//TODO could be triggered more than once
					Result = _necromancerService.Activate (_gameStateService.CurrentGame, heroesToIgnore: new int[] { Result.DetectedHero.Id });
				});
			}
		}

		public RelayCommand Accept{
			get{
				return new RelayCommand (() => {

					_gameStateService.CurrentGame.Necromancer.LocationId = Result.NewLocation.Id;
					//_gameStateService.CurrentGame.Locations.
				});
			}
		}

	}
}

