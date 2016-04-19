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
			DifficultyLevels = (DifficultyLevel[])Enum.GetValues (typeof(DifficultyLevel));

			NumberOfPlayers = 4;
			DifficultyLevel = DifficultyLevel.Adventurer;
        }

		public int NumberOfPlayers { get; set; }

		public DifficultyLevel[] DifficultyLevels { get; private set; }

		DifficultyLevel _difficultyLevel;
		public DifficultyLevel DifficultyLevel 
		{ 
			get { return _difficultyLevel; }
			set{
				if (_difficultyLevel != value) {
					_difficultyLevel = value;
					DifficultyChanged(value);
				}
			}
		}

		DifficultyLevelSettings _difficultyLevelSettings;

		public int StartingDarkness {
			get{ return _difficultyLevelSettings.StartingDarkness; }
			set{ _difficultyLevelSettings.StartingDarkness= value; }
		}
		public int StartingBlights{
			get{ return _difficultyLevelSettings.StartingBlights; }
			set{ _difficultyLevelSettings.StartingBlights= value; }
		}
		public bool PallOfSuffering {
			get{ return _difficultyLevelSettings.PallOfSuffering; }
			set{ _difficultyLevelSettings.PallOfSuffering= value; }
		}
		public bool SpawnExtraQuests {
			get{ return _difficultyLevelSettings.SpawnExtraQuests; }
			set{ _difficultyLevelSettings.SpawnExtraQuests= value; }
			}
			public string Notes {
			get{ return _difficultyLevelSettings.Notes; }
			}

		public bool CanEdit{
			get { return DifficultyLevel == DifficultyLevel.Custom; }
		}

		public void DifficultyChanged(DifficultyLevel difficultyLevel){
			_difficultyLevelSettings = _allDifficultyLevelSettings.Single (x => x.DifficultyLevel == difficultyLevel);
			RaisePropertyChanged (() => StartingDarkness);      
			RaisePropertyChanged (() => StartingBlights);
			RaisePropertyChanged (() => PallOfSuffering);
			RaisePropertyChanged (() => SpawnExtraQuests);
			RaisePropertyChanged (() => Notes);
			RaisePropertyChanged (() => CanEdit);
		}

		protected void UpdateCustomDifficultyLevelSettings(){
			_settings.StartingDarkness = StartingDarkness;
			_settings.StartingBlights = StartingBlights;
			_settings.PallOfSuffering = PallOfSuffering;
			_settings.SpawnExtraQuests = SpawnExtraQuests;
		}

		public RelayCommand ChooseHeroes{
			get{
				return new RelayCommand (async () => {
					Application.CurrentGame = _factory.CreateGameState(_difficultyLevelSettings);
					await Application.Navigation.Push<ChooseHeroesPage>();
					if(_difficultyLevelSettings.DifficultyLevel == DifficultyLevel.Custom)
						UpdateCustomDifficultyLevelSettings();
				});
			}
		}
    }
}
