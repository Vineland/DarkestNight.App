using System;
using System.Linq;
using Vineland.Necromancer.Core;
using System.Runtime.InteropServices;
using Android.Util;
using GalaSoft.MvvmLight.Command;
using Android.Graphics.Drawables;

namespace Vineland.Necromancer.UI
{
	public class NecromancerDetectionViewModel :BaseViewModel
	{
		NecromancerService _necromancerService;
		NavigationService _navigationService;
		GameStateService _gameStateService;


		Hero _wayfarer;

		public NecromancerDetectionViewModel (GameStateService gameStateService, NecromancerService necromancerService, NavigationService navigationService)
		{
			_gameStateService = gameStateService;
			_necromancerService = necromancerService;
			_navigationService = navigationService;

			_wayfarer = _gameStateService.CurrentGame.Heroes.Active.SingleOrDefault (x => x.Name == "Wayfarer");

			var result = _necromancerService.Detect (_gameStateService.CurrentGame);
			DetectedHero = result.DetectedHero;
			MovementRoll = result.MovementRoll;
			DetectionRoll = result.DetectionRoll;
		}

		Hero _detectedHero;
		public Hero DetectedHero {
			get{ return _detectedHero; }
			set {
				if (_detectedHero != value) {
					_detectedHero = value;
					RaisePropertyChanged (() => DetectedHero);
					RaisePropertyChanged (() => BlindingBlackAvailable);
					RaisePropertyChanged (() => DecoyAvailable);
					RaisePropertyChanged (() => ElusiveSpiritAvailable);
				}
			}
		}

		public int DetectionRoll { get; set; }

		public int MovementRoll { get; set; }

		public bool BlindingBlackAvailable {
			get {
				return DetectedHero != null && _gameStateService.CurrentGame.Heroes.BlindingBlackAttained;
			}
		}

		public RelayCommand BlindingBlackCommand {
			get {
				return new RelayCommand (() => {
					DetectedHero = null;
				});
			}
		}

		public bool DecoyAvailable {
			get {
				return DetectedHero != null
					&& _gameStateService.CurrentGame.Heroes.DecoyAttained
				&& (_wayfarer != null
				&& _wayfarer.LocationId != LocationIds.Monastery
				&& _wayfarer.Secrecy >= DetectionRoll);
			}
		}

		public RelayCommand DecoyCommand {
			get {
				return new RelayCommand (() => {
					DetectedHero = _wayfarer;
				});
			}
		}

		public bool ElusiveSpiritAvailable {
			get { 
				return DetectedHero != null
					&& _gameStateService.CurrentGame.Heroes.ElusiveSpiritAttained
				&& DetectionRoll == DetectedHero.Secrecy + 1; 
			}
		}

		public RelayCommand ElusiveSpiritCommand {
			get {
				return new RelayCommand (() => {
					//TODO could be triggered more than once
					var result = _necromancerService.Detect (_gameStateService.CurrentGame, MovementRoll, new int[] { DetectedHero.Id });
					DetectedHero = result.DetectedHero;
				});
			}
		}

		public void MoveAndSpawn ()
		{
			//var spawnResult = _necromancerService.Spawn(
		}

	}
}

