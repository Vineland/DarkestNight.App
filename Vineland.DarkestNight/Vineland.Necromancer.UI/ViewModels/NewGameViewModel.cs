using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vineland.Necromancer.Core;
using Vineland.DarkestNight.UI.Services;
using GalaSoft.MvvmLight;
using Vineland.Necromancer.UI;
using GalaSoft.MvvmLight.Command;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI
{
	public class NewGameViewModel :BaseViewModel
    {
		NavigationService _navigationService;
		GameStateService _gameStateService;
		AppSettings _settings;

		public NewGameViewModel(GameStateService gameStateService, 
			NavigationService navigationService, 
			AppSettings settings)
        {
			_gameStateService = gameStateService;
			_navigationService = navigationService;
			_settings = settings;
            DarknessCardsModeOptions = (DarknessCardsMode[])Enum.GetValues(typeof(DarknessCardsMode));
        }

        public DarknessCardsMode[] DarknessCardsModeOptions { get; private set; }

        public int Darkness
        {
			get { return _settings.StartingDarkness; }
			set { _settings.StartingDarkness = value; }
        }

        public bool PallOfSuffering
        {
			get { return _settings.PallOfSuffering; }
			set { _settings.PallOfSuffering = value; }
        }

        public DarknessCardsMode Mode
        {
			get { return _settings.DarknessCardsMode; }
			set { _settings.DarknessCardsMode = value; }
        }

		public RelayCommand ChooseHeroes{
			get{
				return new RelayCommand (() => {
					_gameStateService.NewGame(_settings);
					_navigationService.Push<ChooseHeroesPage>();
				});
			}
		}
    }
}
