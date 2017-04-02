using System.Collections.ObjectModel;
using Vineland.Necromancer.Domain;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class LocationViewModel:BaseViewModel
	{

		public LocationViewModel (Location location)
		{
			Location = location;
			Spawns = new ObservableCollection<ISpawnViewModel> ();
			foreach (var blight in location.Blights)
				Spawns.Add (new BlightViewModel (blight));

			foreach (var quest in location.Quests)
				Spawns.Add(new QuestViewModel());
		}

		public ObservableCollection<ISpawnViewModel> Spawns { get; private set; }

		public Location Location {get; private set;}

		public virtual Command<BlightViewModel> BlightSelectedCommand {
			get {
				return null;
			}
		}
	}
}

