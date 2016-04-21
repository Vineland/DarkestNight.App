using System;
using System.Collections.ObjectModel;
using Vineland.Necromancer.Core.Services;
using Vineland.Necromancer.Core;
using System.Linq;
using GalaSoft.MvvmLight.Command;

namespace Vineland.Necromancer.UI
{
	public class GameSetupViewModel: BaseViewModel
	{
		D6GeneratorService _d6Generator;

		public GameSetupViewModel (D6GeneratorService d6Generator)
		{
			_d6Generator = d6Generator;
			SpawnLocations = new ObservableCollection<SpawnLocationViewModel> ();
			Initialise ();
		}

		public void Initialise()
		{
			foreach (var location in Application.CurrentGame.Locations)
				SpawnLocations.Add(new SpawnLocationViewModel(location, location.Blights, 0));

			switch (Application.CurrentGame.DifficultyLevel) {
			case DifficultyLevel.Champion:
			case DifficultyLevel.Heroic:
				SpawnLocations.Single (x => x.Location.Id == (int)LocationIds.Village).Add (new QuestSpawnViewModel());
				break;
			case DifficultyLevel.Legendary:
				SpawnRandomQuest ();
				SpawnRandomQuest ();
				break;
			}
		}

		public void SpawnRandomQuest(){
			var locationId = _d6Generator.RollDemBones ();
			var location = SpawnLocations.Single (x => x.Location.Id == locationId);
			location.Add (new QuestSpawnViewModel ());
		}

		public ObservableCollection<SpawnLocationViewModel> SpawnLocations { get; set; }

		public RelayCommand StartGame
		{
			get {
				return new RelayCommand (
					async () => {
						await Application.Navigation.Push<HeroTurnPage>(clearBackStack: true);
					});
			}
		}
	}
}

