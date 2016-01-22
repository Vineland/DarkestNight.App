using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Vineland.DarkestNight.UI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI
{
	public class HomeViewModel : BaseViewModel
    {
        FileService _fileService;
        FileInfo _latestSave;
		NavigationService _navigationService;
		AppSettings _appSettings;
		SaveGameService _saveGameService;

		public HomeViewModel(SaveGameService saveGameService, FileService fileService, NavigationService navigationService, AppSettings appSettings)
        {
			_saveGameService = saveGameService;
            _fileService = fileService;
			_navigationService = navigationService;
			_appSettings = appSettings;

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
					()=> { _navigationService.Push<OptionsPage>();}); 
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
							App.CurrentGame = _saveGameService.CreateDefaultGame();
							_navigationService.Push<ChooseHeroesPage>();
						}else{
							_navigationService.Push<NewGamePage>();
						}
					}); 
			}
		}
    }
}
