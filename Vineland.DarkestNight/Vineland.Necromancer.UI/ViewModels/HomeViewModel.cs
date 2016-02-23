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
		GameStateService _gameStateService;

		public HomeViewModel (GameStateService gameStateService, NavigationService navigationService)
		{
			_gameStateService = gameStateService;
			_navigationService = navigationService;
		}

		public RelayCommand ContinueGameCommand {
			get { 
				return new RelayCommand (
					() => {
						_gameStateService.Continue ();
						_navigationService.Push<HeroPhasePage> ();
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
							_navigationService.Push<NewGamePage> ();
					}); 
			}
		}

		public RelayCommand SettingsCommand{
			get{
				return new RelayCommand (() => {
					_navigationService.Push<SettingsPage>();
				});
			}
		}
	}
}
