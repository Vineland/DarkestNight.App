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


		Hero _warfarer;

		public NecromancerDetectionViewModel (NecromancerService necromancerService, NavigationService navigationService)
		{
			_necromancerService = necromancerService;
			_navigationService = navigationService;

			_warfarer = App.CurrentGame.Heroes.Active.SingleOrDefault(x=>x.Name == "Wayfarer");
			Roll ();
		}

		private Hero _detectedHero;
		public Hero DetectedHero 
		{ 
			get { return _detectedHero; }
			set{
				if (_detectedHero != value) {
					_detectedHero = value;
					RaisePropertyChanged(() => DetectedHero);
				}
			}
		}

		public string DetectedHeroName{
			get{ return DetectedHero == null ? "No hero detected" : DetectedHero.Name; }
		}

		public Location NewLocation { get; set;}
		public DetectionResult Result { get; set;}

		public void Roll(){
			Result = _necromancerService.Detect (App.CurrentGame);

			if(Result.DetectedHeroId.HasValue)
				DetectedHero = App.CurrentGame.Heroes.Active.Single (h => h.Id == Result.DetectedHeroId.Value);
		}

		public bool BlindingBlackAvailable{
			get{
				return DetectedHero != null
				&& App.CurrentGame.Heroes.BlindingBlackAttained;
			}
		}

		public RelayCommand BlindingBlackCommand{
			get {
				return new RelayCommand (() => {
					DetectedHero = null;
				});
			}
		}

		public bool DecoyAvailable{
			get {
				return DetectedHero != null
				&& App.CurrentGame.Heroes.DecoyAttained
					&& (_warfarer != null && _warfarer.Secrecy >= Result.DetectionRoll);
			}
		}

		public RelayCommand DecoyCommand{
			get {
				return new RelayCommand (() => {
					DetectedHero = _warfarer;
				});
			}
		}

		public bool ElusiveSpiritAvailable {
			get { 
				return DetectedHero != null
				&& App.CurrentGame.Heroes.ElusiveSpiritAttained
					&& Result.DetectionRoll == DetectedHero.Secrecy + 1; 
			}
		}

		public RelayCommand ElusiveSpiritCommand{
			get {
				return new RelayCommand (() => {
					//TODO could be triggered more than once
					var result = _necromancerService.Detect (App.CurrentGame, Result.DetectionRoll, new int[] { DetectedHero.Id });
					if (result.DetectedHeroId.HasValue)
						DetectedHero = App.CurrentGame.Heroes.Active.Single (h => h.Id == result.DetectedHeroId.Value);
					else
						DetectedHero = null;
				});
			}
		}

		public void MoveAndSpawn()
		{
			
		}

	}
}

