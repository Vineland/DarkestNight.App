using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class BoardSetupPageModel: BaseViewModel
	{
		public BoardSetupPageModel ()
		{
			Locations = new ObservableCollection<LocationViewModel> ();
			Initialise ();
		}

		public void Initialise()
		{
			foreach (var location in Application.CurrentGame.Locations.Where(x => x.Blights.Any()))
				Locations.Add(new LocationViewModel(location));

		}

		public ObservableCollection<LocationViewModel> Locations { get; set; }

		public Command StartGame
		{
			get {
				return new Command (
					async (x) => {
						//currently the app does not track quests past the initial spwan step
						Application.CurrentGame.Locations.ForEach(l => l.Quests.Clear());
						await CoreMethods.PushPageModel<HeroesPageModel>();

						CoreMethods.RemoveFromNavigation<NewGamePageModel>();
						CoreMethods.RemoveFromNavigation<ChooseHeroesPageModel>();
						CoreMethods.RemoveFromNavigation<BoardSetupPageModel>();
					});
			}
		}
	}
}

