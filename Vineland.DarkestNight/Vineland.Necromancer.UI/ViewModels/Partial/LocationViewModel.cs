using System;
using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.Core;
using System.Collections.ObjectModel;
using System.Linq;

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
		}

		public ObservableCollection<ISpawnViewModel> Spawns { get; private set; }

		public Location Location {get; private set;}

		public virtual RelayCommand<BlightViewModel> BlightSelectedCommand {
			get {
				return null;
			}
		}
	}
}

