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

		public NewGameViewModel(GameStateService gameStateService, 
			NavigationService navigationService, 
			Settings settings)
        {
			_gameStateService = gameStateService;
			_navigationService = navigationService;
			Settings = settings;
            DarknessCardsModeOptions = (DarknessCardsMode[])Enum.GetValues(typeof(DarknessCardsMode));
        }

        public DarknessCardsMode[] DarknessCardsModeOptions { get; private set; }

		public Settings Settings { get; private set; }

		public RelayCommand ChooseHeroes{
			get{
				return new RelayCommand (() => {
					_gameStateService.NewGame(Settings);
					_navigationService.Push<ChooseHeroesPage>();
				});
			}
		}
    }
}
