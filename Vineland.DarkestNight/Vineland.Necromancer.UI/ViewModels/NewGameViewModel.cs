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
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class NewGameViewModel :BaseViewModel
	{
		GameStateService _gameStateService;

		public NewGameViewModel(DataService dataService, GameStateService gameStateService)
        {
			_gameStateService = gameStateService;
			DifficultyLevels = dataService.GetDifficultyLevelSettings ();
			SelectedDifficulty = DifficultyLevels.Single(x => x.Id == DifficultyLevelId.Adventurer);
        }

		public int NumberOfPlayers { get; set; }

		public IEnumerable<DifficultyLevel> DifficultyLevels { get; private set;}

		DifficultyLevel _selectedDifficulty;
		public DifficultyLevel SelectedDifficulty
		{
			get { return _selectedDifficulty;}
			set
			{
				_selectedDifficulty = value;
				RaisePropertyChanged(() => StartingDarkness);
				RaisePropertyChanged(() => StartingBlights);
				RaisePropertyChanged(() => SpawnExtraQuests);
			}
		}

		public int StartingDarkness {
			get{
				return _selectedDifficulty.StartingDarkness;
			}
			set{
				_selectedDifficulty.StartingDarkness = value;
			}
		}

		public int StartingBlights{
			get{
				return _selectedDifficulty.StartingBlights;
			}
			set{
				_selectedDifficulty.StartingBlights = value;
			}
		}

		public bool SpawnExtraQuests
		{
			get { return _selectedDifficulty.SpawnExtraQuests; }
			set
			{
				_selectedDifficulty.SpawnExtraQuests = value;
			}
		}

		public RelayCommand ChooseHeroes{
			get{
				return new RelayCommand (async () => {

					_gameStateService.StartNewGame(NumberOfPlayers, SelectedDifficulty);
					var page = await Application.Navigation.Push<ChooseHeroesPage>();
					(page.BindingContext as ChooseHeroesViewModel).Initialise();
				});
			}
		}
    }
}
