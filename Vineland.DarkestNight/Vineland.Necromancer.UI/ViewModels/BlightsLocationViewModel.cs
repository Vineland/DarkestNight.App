using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using Android.Views.InputMethods;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Vineland.Necromancer.UI
{
	public class BlightLocationsViewModel : BaseViewModel
	{
		BlightService _blightService;

		public BlightLocationsViewModel (BlightService blightService)
		{
			_blightService = blightService;
			Blights = new ObservableCollection<BlightViewModel> ();
			foreach (var location in Application.CurrentGame.Locations) 
			{
				//need to put a dummy one for empty locations for FlowListView to work
				if (!location.Blights.Any ())
					Blights.Add (new BlightViewModel (location, null));
				
				foreach(var blight in location.Blights)
					Blights.Add (new BlightViewModel (location, blight));				
			}
		}

		public ObservableCollection<BlightViewModel> Blights {get;set;}

		public void AddBlight(Location location){
			var blight = _blightService.SpawnBlight (location, Application.CurrentGame);
			Blights.Add (new BlightViewModel (location, blight));
			//yeck, yeck, yeck
			var spacerBlight = Blights.FirstOrDefault(x => x.Location == location && x.Blight == null);
			if (spacerBlight != null)
				Blights.Remove (spacerBlight);
		}

		public void RemoveBlight(BlightViewModel blightViewModel){
			
			if (blightViewModel.Location.Blights.Count == 1)
				Blights.Add(new BlightViewModel (blightViewModel.Location, null));
			
			Blights.Remove (blightViewModel);

			Task.Run (() => {
				_blightService.DestroyBlight (blightViewModel.Location, blightViewModel.Blight, Application.CurrentGame);
				Application.SaveCurrentGame();
			});
		}
	}

	public class BlightViewModel
	{		
		public Location Location { get; protected set; }
		public Blight Blight { get; protected set; }

		public BlightViewModel (Location location, Blight blight)
		{
			Location = location;
			Blight = blight;
		}
	}
}

