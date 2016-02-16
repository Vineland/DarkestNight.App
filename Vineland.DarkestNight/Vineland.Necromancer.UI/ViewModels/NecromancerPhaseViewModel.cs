using System;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class NecromancerPhaseViewModel : BaseViewModel
	{
		NavigationService _navigationService;
		GameStateService _gameStateService;

		public NecromancerPhaseViewModel (NavigationService navigationService, GameStateService gameStateService)
		{
			_navigationService = navigationService;
			_gameStateService = gameStateService;
		}

		public int DarknessLevel{
			get { return _gameStateService.CurrentGame.Darkness; }
			set{ _gameStateService.CurrentGame.Darkness = value; }
		}

		public List<Location> AllLocations{
			get { return Location.All; }
		}

		public NecomancerState Necromancer{
			get{ return _gameStateService.CurrentGame.Necromancer; }
		}

		public RelayCommand Detect{
			get{
				return new RelayCommand (() => {
					_gameStateService.Save();
					_navigationService.Push<NecromancerDetectionPage>();
				});
			}
		}


	}
}

