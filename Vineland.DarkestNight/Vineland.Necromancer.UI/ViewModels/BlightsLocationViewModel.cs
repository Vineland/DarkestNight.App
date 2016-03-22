using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace Vineland.Necromancer.UI
{
	public class BlightLocationsViewModel : BaseViewModel
	{
		BlightService _blightService;

		public BlightLocationsViewModel (BlightService blightService)
		{
			_blightService = blightService;
			var models = Application.CurrentGame.Locations.Select (l => new BlightLocationViewModel (l, blightService, Application));
			LocationSections = new ObservableCollection<BlightLocationViewModel> (models);
		}

		public ObservableCollection<BlightLocationViewModel> LocationSections { get; set; }

		public async void BlightRowSelected (BlightRowViewModel selectedBlightRow)
		{
			
			var action = await Application.Navigation.DisplayActionSheet (string.Empty, "Cancel", string.Empty, "Destroy", "Move");
			if (action == "Destroy")
				DestroyBlight (selectedBlightRow);
				
			if (action == "Move") {
				var newLocationName = await Application.Navigation.DisplayActionSheet ("New Location", "Cancel", string.Empty, Application.CurrentGame.Locations.Select (x => x.Name).ToArray ());
				if (newLocationName != "Cancel") {
					MoveBlight (selectedBlightRow, newLocationName);
				}
			}
		}

		public void DestroyBlight (BlightRowViewModel selectedRow)
		{
			
			var locationSection = LocationSections.Single (l => l.Contains (selectedRow));
			locationSection.Remove (selectedRow);

			Task.Run (() => {				
				_blightService.DestroyBlight (locationSection.Location, selectedRow.Blight, Application.CurrentGame);
				Application.SaveCurrentGame ();
			});
		}

		public void MoveBlight (BlightRowViewModel rowToMove, string newLocation)
		{

			var currentLocationSection = LocationSections.Single (l => l.Contains (rowToMove));
			if (currentLocationSection.Location.Name == newLocation)
				return;

			var newLocationSection = LocationSections.Single (l => l.Location.Name == newLocation);
			currentLocationSection.Remove (rowToMove);
			newLocationSection.Insert (0, rowToMove);

			Task.Run (() => {
				currentLocationSection.Location.Blights.Remove (rowToMove.Blight);
				newLocationSection.Location.Blights.Add (rowToMove.Blight);
				Application.SaveCurrentGame ();
			});
		}
	}

	public class BlightLocationViewModel : ObservableCollection<BlightRowViewModel>
	{
		public Location Location { get; private set; }

		NecromancerApp _application;
		BlightService _blightService;

		public BlightLocationViewModel (Location location, BlightService blightService, NecromancerApp application)
		{
			Location = location;
			_application = application;
			_blightService = blightService;

			foreach (var blight in location.Blights)
				this.Add (new BlightRowViewModel (blight));			
		}

		public RelayCommand AddBlightCommand {
			get { 
				return new RelayCommand (async () => {
				
					var action = await _application.Navigation.DisplayActionSheet (
						"New Blight", 
						"Cancel", 
						string.Empty, 
						"Spawn", 
						"Select");
					switch (action) 
					{
						case "Spawn":
							SpawnBlight ();
							break;
						case "Select":
							SelectBlight();
							break;						
					}
				}, 
				()=>{
						return this.Sum(x=>x.Blight.Weight) < 4;
				});
			}
		}


		public void SpawnBlight ()
		{
			var blight = _blightService.SpawnBlight (Location, _application.CurrentGame).Item2;

			this.Add (new BlightRowViewModel (blight));

			Task.Run (() => {
				_application.SaveCurrentGame ();
			});
		}

		public async void SelectBlight(){
			var blightOptions = _application.CurrentGame.BlightPool.Select (x => x.Name).Distinct ();
			var option = await _application.Navigation.DisplayActionSheet ("Select Blight", "Cancel", null, blightOptions.ToArray ());
			if (option == "Cancel")
				return;

			var blight = _application.CurrentGame.BlightPool.FirstOrDefault(x=>x.Name == option);
			this.Add (new BlightRowViewModel (blight));
			Task.Run (() => {
				_application.CurrentGame.BlightPool.Remove(blight);
				Location.Blights.Add(blight);
				_application.SaveCurrentGame ();
			});
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
			get{ return Blight.Name; }
		}

		public string ImageName {
			get { return string.Format ("blight_{0}", Blight.Name.ToLower ().Replace (" ", "_")); }
		}
	}
}

