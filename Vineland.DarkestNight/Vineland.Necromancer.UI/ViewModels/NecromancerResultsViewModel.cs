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

		public NecromancerResultsViewModel (NecromancerService necromancerService)
		{
			_necromancerService = necromancerService;

			var results = new List<NecromancerActivationResult> ();
			if (Application.CurrentGame.Heroes.ProphecyOfDoomRoll == 0)
				results.Add (_necromancerService.Activate (Application.CurrentGame));
			else
				results.Add (_necromancerService.Activate (Application.CurrentGame, roll: Application.CurrentGame.Heroes.ProphecyOfDoomRoll));

			if (Application.CurrentGame.Heroes.RuneOfMisdirectionActive)
				results.Add (_necromancerService.Activate (Application.CurrentGame));

			Results = results.Select (x => new NecromancerActivationResultViewModel (x, _necromancerService)).ToList();
			SelectedResult = Results.First ();
		}

		public List<NecromancerActivationResultViewModel> Results { get; set; }

		public NecromancerActivationResultViewModel SelectedResult { get; set; }


		public RelayCommand Accept {
			get {
				return new RelayCommand (() => {
					
					Application.CurrentGame.Heroes.ProphecyOfDoomRoll = 0;
					Application.CurrentGame.Necromancer.LocationId = SelectedResult.Result.NewLocation.Id;
					//TODO spawn blights
					//Application.CurrentGame.Locations.Single (x => x.Id == SelectedResult.Result.NewLocation.Id).NumberOfBlights += SelectedResult.Result.NumberOfBlightsToNewLocation;
					//Application.CurrentGame.Locations.Single (x => x.Id == LocationIds.Monastery).NumberOfBlights += SelectedResult.Result.NumberOfBlightsToMonastery;
					Application.SaveCurrentGame ();

					Application.Navigation.PopTo<HeroPhasePage> ();
				});
			}
		}
	}

	public class NecromancerActivationResultViewModel : BaseViewModel
	{

		NecromancerActivationResult _result;
		NecromancerService _necromancerService;

		public NecromancerActivationResultViewModel (NecromancerActivationResult result, NecromancerService necroService)
		{
			_result = result;
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
				return Application.CurrentGame.Heroes.BlindingBlackActive;
			}
		}

		public RelayCommand BlindingBlackCommand {
			get {
				return new RelayCommand (() => {
					Result = _necromancerService.Activate (Application.CurrentGame, roll: Result.MovementRoll, heroesToIgnore: Application.CurrentGame.Heroes.Active.Select (x => x.Id).ToArray ());
				},
					() => {
						return Result.DetectedHero != null;
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
					var wayfarer = Application.CurrentGame.Heroes.Active.SingleOrDefault (x => x.Name == "Wayfarer");
					Result = _necromancerService.Activate (Application.CurrentGame, detectedHero: wayfarer);
				},
					() => {
						var wayfarer = Application.CurrentGame.Heroes.Active.SingleOrDefault (x => x.Name == "Wayfarer");

						return Result.DetectedHero != null
						&& (wayfarer != null
						&& wayfarer.LocationId != LocationIds.Monastery
						&& wayfarer.Secrecy >= Result.DetectionRoll);
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
					//TODO could be triggered more than once
					Result = _necromancerService.Activate (Application.CurrentGame, heroesToIgnore: new int[] { Result.DetectedHero.Id });
				},
					() => {
						return Result.DetectedHero != null && Result.DetectionRoll == Result.DetectedHero.Secrecy + 1; 
					});
			}
		}
	}
}

