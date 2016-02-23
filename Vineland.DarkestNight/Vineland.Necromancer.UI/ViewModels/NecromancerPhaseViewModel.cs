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

		public NecromancerPhaseViewModel (NavigationService navigationService, 
			GameStateService gameStateService,
			Settings settings)
		{
			_navigationService = navigationService;
			_gameStateService = gameStateService;
		}

		public Settings Settings { get; private set; }

		public bool ShowDarknessCardOptions{
			get { return _gameStateService.CurrentGame.Mode != DarknessCardsMode.None; }
		}

		public int Darkness{
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
					_navigationService.Push<NecromancerResultsPage>();
				});
			}
		}
	}
}

