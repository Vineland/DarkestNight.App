using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Vineland.DarkestNight.UI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.UI;

namespace Vineland.DarkestNight.UI.ViewModels
{
	public class HomeViewModel : ViewModelBase
    {
        FileService _fileService;
        FileInfo _latestSave;
		NavigationService _navigationService;
		AppSettings _appSettings;
		AppGameState _gameState;

		public HomeViewModel(FileService fileService, NavigationService navigationService, AppSettings appSettings, AppGameState gameState)
        {
            _fileService = fileService;
			_navigationService = navigationService;
			_appSettings = appSettings;
			_gameState = gameState;

			Initialise ();
        }

        public void Initialise()
        {
			_latestSave = _fileService.SearchDirectory(AppConstants.SavesLocation, "*" + AppConstants.SaveFileExtension)
                .OrderByDescending(x => x.CreationTime)
                .FirstOrDefault();
        }

		public RelayCommand ContinueGameCommand
		{
			get 
			{ 
				return new RelayCommand (
				()=> { /*TODO: set game state to latest save}*/},
				() => { return _latestSave != null;}); 
			}
		}

		public RelayCommand LoadGameCommand
		{
			get 
			{ 
				return new RelayCommand (
					()=> { },
					() => { return _latestSave != null;}); 
			}
		}

		public RelayCommand OptionsCommand
		{
			get 
			{ 
				return new RelayCommand (
					()=> { _navigationService.NavigateToViewModel<OptionsViewModel>();}); 
			}
		}

		public RelayCommand NewGameCommand
		{
			get 
			{ 
				return new RelayCommand (
					()=> { 
						if(_appSettings.AlwaysUseDefaults)
						{
							_gameState.LoadDefaults(_appSettings);
							_navigationService.NavigateToViewModel<ChooseHeroesViewModel>();
						}else{
						_navigationService.NavigateToViewModel<NewGameViewModel>();
						}
					}); 
			}
		}
    }
}
