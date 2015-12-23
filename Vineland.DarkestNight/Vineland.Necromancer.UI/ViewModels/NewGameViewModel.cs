using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vineland.DarkestNight.Core;
using Vineland.DarkestNight.UI.Services;
using GalaSoft.MvvmLight;
using Vineland.Necromancer.UI;
using GalaSoft.MvvmLight.Command;

namespace Vineland.DarkestNight.UI.ViewModels
{
	public class NewGameViewModel :ViewModelBase
    {
        AppSettings _appSettings;
        AppGameState _gameState;
		NavigationService _navigationService;

		public NewGameViewModel(AppSettings appSettings, AppGameState gameState, NavigationService navigationService)
        {
            _appSettings = appSettings;
            _gameState = gameState;
			_navigationService = navigationService;
            DarknessCardsModeOptions = (DarknessCardsMode[])Enum.GetValues(typeof(DarknessCardsMode));

			Initialise ();
        }
        
        public void Initialise()
        {
			_gameState.LoadDefaults (_appSettings);
        }

        public DarknessCardsMode[] DarknessCardsModeOptions { get; private set; }

        public int DarknessLevel
        {
            get { return _gameState.DarknessLevel; }
            set { _gameState.DarknessLevel = value; }
        }

        public bool PallOfSuffering
        {
            get { return _gameState.PallOfSuffering; }
            set { _gameState.PallOfSuffering = value; }
        }

        public DarknessCardsMode Mode
        {
            get { return _gameState.Mode; }
            set { _gameState.Mode = value; }
        }

		public RelayCommand ChooseHeroes{
			get{
				return new RelayCommand (() => {
					_gameState.Save();
					_navigationService.NavigateToViewModel<ChooseHeroesViewModel>();
				});
			}
		}
    }
}
