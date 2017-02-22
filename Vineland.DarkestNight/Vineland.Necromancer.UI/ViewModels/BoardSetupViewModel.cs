using System;
using System.Collections.ObjectModel;
using Vineland.Necromancer.Core.Services;
using Vineland.Necromancer.Core;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class BoardSetupViewModel: BaseViewModel
	{
		D6GeneratorService _d6Generator;

		public BoardSetupViewModel (D6GeneratorService d6Generator)
		{
			_d6Generator = d6Generator;
			Locations = new ObservableCollection<LocationViewModel> ();
			Initialise ();
		}

		public void Initialise()
		{
			foreach (var location in Application.CurrentGame.Locations.Where(x => x.Blights.Any()))
				Locations.Add(new LocationViewModel(location));

		}

		public ObservableCollection<LocationViewModel> Locations { get; set; }

		public RelayCommand StartGame
		{
			get {
				return new RelayCommand (
					async () => {
						//currently the app does not track quests past the initial spwan step
						Application.CurrentGame.Locations.ForEach(l => l.Quests.Clear());
					await Application.Navigation.Push<HeroesPage>(clearBackStack: true);
					});
			}
		}
	}
}

