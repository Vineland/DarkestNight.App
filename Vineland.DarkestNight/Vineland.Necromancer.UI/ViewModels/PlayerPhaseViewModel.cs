using System;
using GalaSoft.MvvmLight.Command;

namespace Vineland.Necromancer.UI
{
	public class PlayerPhaseViewModel : BaseViewModel
	{
		NavigationService _navigationService;
		SaveGameService _saveGameService;

		public PlayerPhaseViewModel (NavigationService navigationService, SaveGameService saveGameService)
		{
			_saveGameService = saveGameService;
			_navigationService = navigationService;
		}

		public RelayCommand NextPhase{
			get{
				return new RelayCommand (() => {
					_saveGameService.Save(App.CurrentGame);
					_navigationService.Push<NecromancerPhasePage>();
				});
			}
		}
	}
}

