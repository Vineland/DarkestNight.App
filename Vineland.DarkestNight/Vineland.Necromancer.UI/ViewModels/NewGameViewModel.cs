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
using System.Collections.ObjectModel;
using System.Linq;

namespace Vineland.Necromancer.UI
{
	public class NewGameViewModel :BaseViewModel
	{
		GameStateFactory _factory;
		List<DifficultyLevelSettings> _allDifficultyLevelSettings;
		Settings _settings;

		public NewGameViewModel(DataService dataService, Settings settings, GameStateFactory factory)
        {
			_settings = settings;
			_factory = factory;
			_allDifficultyLevelSettings = dataService.GetDifficultyLevelSettings ();

			DarknessCardsModes = (DarknessCardsMode[])Enum.GetValues(typeof(DarknessCardsMode));

        }

		DifficultyLevelSettings _difficultyLevelSettings;

		public int StartingDarkness {
			get{
				return _settings.StartingDarkness;
			}
			set{
				_settings.StartingDarkness = value;
			}
		}

		public int StartingBlights{
			get{
				return _settings.StartingBlights;
			}
			set{
				_settings.StartingBlights = value;
			}
		}

		public bool UseQuests
		{
			get { return _settings.UseQuests; }
			set
			{
				_settings.UseQuests = value;}
		}

		public DarknessCardsMode[] DarknessCardsModes { get; private set;}
		public DarknessCardsMode DarknessCardsMode
		{
			get { return _settings.DarknessCardsMode;}
			set
			{
				_settings.DarknessCardsMode = value;
			}
		}

		public RelayCommand ChooseHeroes{
			get{
				return new RelayCommand (async () => {
					
					Application.CurrentGame = _factory.CreateGameState(4, StartingDarkness, StartingBlights, UseQuests, DarknessCardsMode);
					var page = await Application.Navigation.Push<ChooseHeroesPage>();
					(page.BindingContext as ChooseHeroesViewModel).Initialise();
				});
			}
		}
    }
}
