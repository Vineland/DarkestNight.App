using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Vineland.DarkestNight.UI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Vineland.DarkestNight.UI;
using Android.InputMethodServices;

namespace Vineland.Necromancer.UI
{
	public class HomeViewModel : BaseViewModel
	{
		NavigationService _navigationService;
		AppSettings _appSettings;
		GameStateService _gameStateService;

		public HomeViewModel (GameStateService gameStateService, NavigationService navigationService, AppSettings appSettings)
		{
			_gameStateService = gameStateService;
			_navigationService = navigationService;
			_appSettings = appSettings;
		}

		public RelayCommand ContinueGameCommand {
			get { 
				return new RelayCommand (
					() => {
						_gameStateService.Continue ();
						_navigationService.Push<PlayerPhasePage> ();
					},
					() => {
						return _gameStateService.CurrentGame != null;
					}); 
			}
		}

		public RelayCommand NewGameCommand {
			get { 
				return new RelayCommand (
					() => { 

						_gameStateService.NewGame (_appSettings);
						if (_appSettings.AlwaysUseDefaults)
							_navigationService.Push<ChooseHeroesPage> ();
						else
							_navigationService.Push<NewGamePage> ();
					}); 
			}
		}
	}
}
