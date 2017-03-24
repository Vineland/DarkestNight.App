using System.Collections.Generic;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
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
			NumberOfPlayers = 4;
			NumberOfDarknessCards = 2;
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

		bool _usingDarknessCards;
		public bool UsingDarknessCards
		{
			get { return _usingDarknessCards; }
			set
			{
				_usingDarknessCards = value;
				RaisePropertyChanged(() => UsingDarknessCards);
			}
		}

		public int NumberOfDarknessCards { get; set; }

		public RelayCommand ChooseHeroes{
			get{
				return new RelayCommand (async () => 
				{
					Application.CurrentGame =_gameStateService.StartNewGame(NumberOfPlayers, NumberOfDarknessCards, SelectedDifficulty);
					var page = await Application.Navigation.Push<ChooseHeroesPage>();
					(page.BindingContext as ChooseHeroesViewModel).Initialise();
				});
			}
		}
    }
}
