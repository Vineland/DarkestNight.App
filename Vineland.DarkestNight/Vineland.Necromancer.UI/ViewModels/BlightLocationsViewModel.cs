using System;
using System.Collections.Generic;
using Android.Locations;
using GalaSoft.MvvmLight.Command;

namespace Vineland.Necromancer.UI
{
	public class BlightLocationsViewModel : BaseViewModel
	{
		NavigationService _navigationService;
		SaveGameService _saveGameService;

		public BlightLocationsViewModel (NavigationService navigationService, SaveGameService saveGameService)
		{
			_navigationService = navigationService;
			_saveGameService = saveGameService;
		}

		public List<Location> Locations{
			get { return App.CurrentGame.Locations; }
		}

		public RelayCommand NextCommand{
			get{
				new RelayCommand (() => {
					_saveGameService.Save(App.CurrentGame);
					_navigationService.PushViewModel<NecromancerPhaseViewModel>();
				});
			}
		}
	}
}

