using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using Android.Views.InputMethods;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.Database;
using System.Runtime.InteropServices;

namespace Vineland.Necromancer.UI
{
	public class BlightLocationsViewModel : BaseViewModel
	{
		BlightService _blightService;

		public BlightLocationsViewModel (BlightService blightService)
		{
			_blightService = blightService;
			var models = Application.CurrentGame.Locations.Select (x => new BlightLocationViewModel (x.Name, x.Blights));
			Locations = new ObservableCollection<BlightLocationViewModel> (models);
		}

		public ObservableCollection<BlightViewModel> Blights { get; set; }
		public ObservableCollection<BlightLocationViewModel> Locations { get; set; }

//		public void AddBlight (Location location)
//		{
//			var blight = _blightService.SpawnBlight (location, Application.CurrentGame);
//			Blights.Add (new BlightViewModel () { Location = location, Blight = blight });
//			//yeck, yeck, yeck
//			var spacerBlight = Blights.FirstOrDefault (x => x.Location == location && x.Blight == null);
//			if (spacerBlight != null)
//				Blights.Remove (spacerBlight);
//		}

		public void RemoveBlight (BlightViewModel blightViewModel)
		{
			
			if (blightViewModel.Location.Blights.Count == 1)
				Blights.Add (new BlightViewModel () { Location = blightViewModel.Location });
			
			Blights.Remove (blightViewModel);

			Task.Run (() => {
				_blightService.DestroyBlight (blightViewModel.Location, blightViewModel.Blight, Application.CurrentGame);
				Application.SaveCurrentGame ();
			});
		}
	}

	public class BlightLocationViewModel : ObservableCollection<Blight>
	{
		public string LocationName { get; private set; }        

		public BlightLocationViewModel(string locationName, IEnumerable<Blight> blights)
			:base(blights)
		{
			LocationName = locationName;      
		}
	}


	public class BlightViewModel
	{
		public Location Location { get; set; }

		public Blight Blight { get; set; }

		public BlightViewModel ()
		{
		}
	}
}

