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

			var results = new List<NecromancerDetectionResult> ();
			if (_seer == null || _seer.ProphecyOfDoomRoll == 0)
				results.Add (_necromancerService.Activate (Application.CurrentGame));
			else
				results.Add (_necromancerService.Activate (Application.CurrentGame, roll: _seer.ProphecyOfDoomRoll));

			if (_wizard != null && _wizard.RuneOfMisdirectionActive)
				results.Add (_necromancerService.Activate (Application.CurrentGame));

			Results = results.Select (x => new NecromancerDetectionResultViewModel (x, _necromancerService)).ToList ();

			SelectedResult = Results.First ();
		}

		public List<NecromancerDetectionResultViewModel> Results { get; set; }

		public NecromancerDetectionResultViewModel SelectedResult { get; set; }


		public RelayCommand SpawnBlightsCommand {
			get {
				return new RelayCommand (() => {
					
					_seer.ProphecyOfDoomRoll = 0;
					Application.CurrentGame.Necromancer.LocationId = SelectedResult.Result.NewLocation.Id;
					Application.SaveCurrentGame ();

					var viewModel = Resolver.Resolve<NecromancerSpawnViewModel>();
					viewModel.Initialise(SelectedResult.Result.NumberOfBlightsToNewLocation, SelectedResult.Result.NumberOfBlightsToMonastery);

					Application.Navigation.Push<NecromancerSpawnPage> (viewModel);
				});
			}
		}
	}

	public class NecromancerDetectionResultViewModel : BaseViewModel
	{

		NecromancerDetectionResult _result;
		NecromancerService _necromancerService;
		Acolyte _acolyte;
		Wayfarer _wayfarer;
		Valkyrie _valkyrie;
		Shaman _shaman;

		public NecromancerDetectionResultViewModel(NecromancerDetectionResult result, NecromancerService necroService)
		{
			_result = result;
			_necromancerService = necroService;
			_acolyte = Application.CurrentGame.Heroes.GetHero<Acolyte> ();
			_wayfarer = Application.CurrentGame.Heroes.GetHero <Wayfarer> ();
			_valkyrie = Application.CurrentGame.Heroes.GetHero <Valkyrie> ();
			_shaman = Application.CurrentGame.Heroes.GetHero<Shaman> ();
		}

		public NecromancerDetectionResult Result {
			get{ return _result; }
			set {
				_result = value;
				RaisePropertyChanged (() => Result);
			}
		}



//		public RelayCommand IgnoreAllHeroesCommand {
//			get {
//				return new RelayCommand (() => {
//					Result = _necromancerService.Activate (Application.CurrentGame, roll: Result.MovementRoll, heroesToIgnore: Application.CurrentGame.Heroes.Select (x => x.Id).ToArray ());
//				},
//					() => {
//						return Result.DetectedHero != null;
//					});
//			}
//		}
//
//		List<int> _heroesToIgnore = new List<int> ();
//
//		public RelayCommand IgnoreHeroCommand {
//			get {
//				return new RelayCommand (() => {
//					_heroesToIgnore.Add (Result.DetectedHero.Id);
//					Result = _necromancerService.Activate (Application.CurrentGame, roll: Result.MovementRoll, heroesToIgnore: _heroesToIgnore);
//				},
//					() => {
//						return Result.DetectedHero != null;
//					});
//			}
//		}
				public RelayCommand BlindingBlackCommand {
					get {
						return new RelayCommand (() => {
							Result = _necromancerService.Activate (Application.CurrentGame, roll: Result.MovementRoll, heroesToIgnore: Application.CurrentGame.Heroes.Select (x => x.Id).ToArray ());
							_acolyte.BlindingBlackActive = false;
						},
							() => {
								return Result.DetectedHero != null;
							});
					}
				}
		
				public bool DecoyVisible {
					get {
						return _wayfarer != null && _wayfarer.DecoyActive;
					}
				}
		
				public RelayCommand DecoyCommand {
					get {
						return new RelayCommand (() => {
							Result = _necromancerService.Activate (Application.CurrentGame, detectedHero: _wayfarer);
						},
							() => {
								return Result.DetectedHero != null
								&& (_wayfarer != null
								&& _wayfarer.LocationId != LocationIds.Monastery
								&& _wayfarer.Secrecy >= Result.DetectionRoll);
							});
					}
				}
		
				public bool ElusiveSpiritVisible {
					get {
						return _valkyrie != null && _valkyrie.ElusiveSpiritActive;
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

