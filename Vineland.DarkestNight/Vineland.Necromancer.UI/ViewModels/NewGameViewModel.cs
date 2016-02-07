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

		public NewGameViewModel(GameStateService gameStateService, NavigationService navigationService)
        {
			_gameStateService = gameStateService;
			_navigationService = navigationService;
            DarknessCardsModeOptions = (DarknessCardsMode[])Enum.GetValues(typeof(DarknessCardsMode));
        }

        public DarknessCardsMode[] DarknessCardsModeOptions { get; private set; }

        public int DarknessLevel
        {
			get { return _gameStateService.CurrentGame.DarknessLevel; }
			set { _gameStateService.CurrentGame.DarknessLevel = value; }
        }

        public bool PallOfSuffering
        {
			get { return _gameStateService.CurrentGame.PallOfSuffering; }
			set { _gameStateService.CurrentGame.PallOfSuffering = value; }
        }

        public DarknessCardsMode Mode
        {
			get { return _gameStateService.CurrentGame.Mode; }
			set { _gameStateService.CurrentGame.Mode = value; }
        }

		public RelayCommand ChooseHeroes{
			get{
				return new RelayCommand (() => {
					_gameStateService.Save();
					_navigationService.Push<ChooseHeroesPage>();
				});
			}
		}
    }
}
