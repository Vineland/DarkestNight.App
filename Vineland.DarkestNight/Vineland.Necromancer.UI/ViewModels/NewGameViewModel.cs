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
		SaveGameService _saveGameService;

		public NewGameViewModel(SaveGameService saveGameService, NavigationService navigationService)
        {
			_saveGameService = saveGameService;
			_navigationService = navigationService;
            DarknessCardsModeOptions = (DarknessCardsMode[])Enum.GetValues(typeof(DarknessCardsMode));

			App.CurrentGame = _saveGameService.CreateDefaultGame ();
        }

        public DarknessCardsMode[] DarknessCardsModeOptions { get; private set; }

        public int DarknessLevel
        {
			get { return App.CurrentGame.DarknessLevel; }
			set { App.CurrentGame.DarknessLevel = value; }
        }

        public bool PallOfSuffering
        {
			get { return App.CurrentGame.PallOfSuffering; }
			set { App.CurrentGame.PallOfSuffering = value; }
        }

        public DarknessCardsMode Mode
        {
			get { return App.CurrentGame.Mode; }
			set { App.CurrentGame.Mode = value; }
        }

		public RelayCommand ChooseHeroes{
			get{
				return new RelayCommand (() => {
					_saveGameService.Save(App.CurrentGame);
					_navigationService.PushViewModel<ChooseHeroesViewModel>();
				});
			}
		}
    }
}
