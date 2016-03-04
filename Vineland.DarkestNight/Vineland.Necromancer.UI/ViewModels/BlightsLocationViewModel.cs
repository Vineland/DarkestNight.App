using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using Android.Views.InputMethods;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.Database;

namespace Vineland.Necromancer.UI
{
	public class BlightLocationsViewModel : BaseViewModel
	{
		BlightService _blightService;

		public BlightLocationsViewModel (BlightService blightService)
		{
			_blightService = blightService;
//			var blights = new ObservableCollection<BlightViewModel> ();
//			foreach (var location in Application.CurrentGame.Locations) {
//				//need to put a dummy one for empty locations for FlowListView to work
//				if (!location.Blights.Any ())
//					blights.Add (new BlightViewModel  { Location = location });
//				
//				foreach (var blight in location.Blights)
//					blights.Add (new BlightViewModel  { Location = location, Blight = blight });				
//			}
//			Blights = blights;
			Locations = new ObservableCollection<LocationViewModel> ();
			foreach (var location in Application.CurrentGame.Locations) {
				var locationVM = new LocationViewModel (location.Name);
				foreach (var blight in location.Blights)
					locationVM.Add (blight);

				Locations.Add (locationVM);
			}
		}

		public ObservableCollection<BlightViewModel> Blights { get; set; }
		public ObservableCollection<LocationViewModel> Locations { get; set; }

		public void AddBlight (Location location)
		{
			var blight = _blightService.SpawnBlight (location, Application.CurrentGame);
			Blights.Add (new BlightViewModel () { Location = location, Blight = blight });
			//yeck, yeck, yeck
			var spacerBlight = Blights.FirstOrDefault (x => x.Location == location && x.Blight == null);
			if (spacerBlight != null)
				Blights.Remove (spacerBlight);
		}

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

	public class LocationViewModel : ObservableCollection<Blight>
	{
		public String Name { get; private set; }        

		public LocationViewModel(String Name)
		{
			this.Name = Name;            
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

