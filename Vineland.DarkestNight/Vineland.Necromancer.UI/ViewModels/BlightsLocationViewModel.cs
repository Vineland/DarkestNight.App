using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
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
			var models = Application.CurrentGame.Locations.Select (l => new BlightLocationViewModel (l));
			LocationSections = new ObservableCollection<BlightLocationViewModel> (models);
		}

		public ObservableCollection<BlightLocationViewModel> LocationSections { get; set; }

		public async void BlightRowSelected(BlightRowViewModel selectedBlightRow){
			if (selectedBlightRow.IsPlaceholder)
				SpawnBlight (selectedBlightRow);
			else {
				var action = await Application.Navigation.DisplayActionSheet (string.Empty, "Cancel", string.Empty, "Destroy", "Move");
				if (action == "Destroy")
					DestoryBlight (selectedBlightRow);
				
				if(action=="Move")
				{
					var newLocationName = await Application.Navigation.DisplayActionSheet ("New Location", "Cancel", string.Empty, Application.CurrentGame.Locations.Select (x => x.Name).ToArray ());
					if (newLocationName != "Cancel") {
						MoveBlight (selectedBlightRow, newLocationName);
					}
				}
			}
		}

		public void SpawnBlight (BlightRowViewModel spawnRow)
		{
			var locationSection = LocationSections.Single (l => l.Contains (spawnRow));

			var blight = _blightService.SpawnBlight (locationSection.Location, Application.CurrentGame).Item2;

			locationSection.Insert (locationSection.Count - 1, new BlightRowViewModel (blight));
			if (locationSection.Location.BlightCount >= 4)
				locationSection.Remove (spawnRow);

			Task.Run (() => {
				Application.SaveCurrentGame ();
			});
		}

		public void DestoryBlight (BlightRowViewModel selectedRow)
		{
			
			var locationSection = LocationSections.Single (l => l.Contains (selectedRow));
			locationSection.Remove (selectedRow);
			if (locationSection.Count < 4
			    && !locationSection.Any (x => x.IsPlaceholder))
				locationSection.Add (new BlightRowViewModel (null));

			Task.Run (() => {				
				_blightService.DestroyBlight (locationSection.Location, selectedRow.Blight, Application.CurrentGame);
				Application.SaveCurrentGame ();
			});
		}

		public void MoveBlight(BlightRowViewModel rowToMove, string newLocation){

			var currentLocationSection = LocationSections.Single (l => l.Contains (rowToMove));
			if (currentLocationSection.Location.Name == newLocation)
				return;

			var newLocationSection = LocationSections.Single (l => l.Location.Name == newLocation);
			currentLocationSection.Remove (rowToMove);
			newLocationSection.Insert (0, rowToMove);

			Task.Run (() => {
				currentLocationSection.Location.Blights.Remove(rowToMove.Blight);
				newLocationSection.Location.Blights.Add(rowToMove.Blight);
				Application.SaveCurrentGame();
			});
		}
	}

	public class BlightLocationViewModel : ObservableCollection<BlightRowViewModel>
	{
		public Location Location { get; private set; }

		public BlightLocationViewModel (Location location)
		{
			Location = location;

			foreach (var blight in location.Blights) {
				this.Add (new BlightRowViewModel (blight));
			}

			if (location.BlightCount < 4)
				this.Add (new BlightRowViewModel (null));
			
		}
	}

	public class BlightRowViewModel : BaseViewModel
	{
		public Blight Blight{ get ; private set; }

		public BlightRowViewModel (Blight blight)
		{
			Blight = blight;
		}

		public string Name {
			get{ return Blight == null ? "Spawn Blight" : Blight.Name; }
		}

		public string ImageName {
			get { return Blight == null ? "plus" : string.Format ("blight_{0}", Blight.Name.ToLower ().Replace (" ", "_")); }
		}

		public bool IsPlaceholder { get { return Blight == null; } }
	}
}

