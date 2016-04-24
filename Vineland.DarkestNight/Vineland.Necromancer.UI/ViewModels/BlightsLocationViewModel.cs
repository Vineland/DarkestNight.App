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
		public BlightService BlightService;

		public BlightLocationsViewModel (BlightService blightService)
		{
			BlightService = blightService;
			var models = Application.CurrentGame.Locations.Select (l => new LocationViewModel (this, l));
			LocationSections = new ObservableCollection<LocationViewModel> (models);

			MessagingCenter.Subscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete", OnNecromancerPhaseComplete);
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete");
		}

		public void OnNecromancerPhaseComplete(NecromancerActivationViewModel sender)
		{
			//TODO: only add the new rows
			var models = Application.CurrentGame.Locations.Select (l => new LocationViewModel (this, l));
			LocationSections.Clear ();
			foreach(var model in models)
				LocationSections.Add(model);
		}

		public ObservableCollection<LocationViewModel> LocationSections { get; set; }
	}

	public class LocationViewModel : BaseViewModel
	{
		public Location Location { get; private set; }

		BlightLocationsViewModel _parent;

		public LocationViewModel (
			BlightLocationsViewModel parent,
			Location location)
		{
			Location = location;
			_parent = parent;

			Blights = new ObservableCollection<BlightViewModel> ();
			Blights.Add (new BlightViewModel (null));
			foreach (var blight in location.Blights)
				AddBlightViewModel (new BlightViewModel (blight));
		}

		public ObservableCollection<BlightViewModel> Blights {get;private set;}

		public void AddBlightViewModel(BlightViewModel viewModel){
			if  (Blights.Any() && Blights.Last().IsPlaceHolder)
				Blights.Insert (Blights.Count - 1, viewModel);
			else
				Blights.Add(viewModel);

			if (Blights.Where (x => !x.IsPlaceHolder).Sum (x => x.Blight.Weight) >= 4
				&& Blights.Last().IsPlaceHolder)
				Blights.RemoveAt (Blights.Count - 1);
		}

		public void RemoveBlightViewModel(BlightViewModel viewModel){
			if (!Blights.Contains (viewModel))
				return;
				
			Blights.Remove (viewModel);

			if (Blights.Where (x => !x.IsPlaceHolder).Sum (x => x.Blight.Weight) < 4
				&& !Blights.Last().IsPlaceHolder)
				Blights.Add (new BlightViewModel (null));
		}

		public RelayCommand<BlightViewModel> BlightSelectedCommand{
			get{
				return new RelayCommand<BlightViewModel> (async (blightViewModel) => {
					if (blightViewModel.Blight != null) {
						var action = await Application.Navigation.DisplayActionSheet ("Remove Blight", "Cancel", null, "Destroy", "Move");
						if (action == "Destroy")
							DestroyBlight (blightViewModel);

						if (action == "Move") {
							var newLocationName = await Application.Navigation.DisplayActionSheet ("New Location", "Cancel", null, Application.CurrentGame.Locations.Select (x => x.Name).ToArray ());
							if (newLocationName != "Cancel") {
								MoveBlight (blightViewModel, newLocationName);
							}
						}
					} else {
						var action = await Application.Navigation.DisplayActionSheet (
							              "New Blight", 
							              "Cancel", 
							              null, 
							              "Spawn", 
							              "Select");
						switch (action) {
						case "Spawn":
							SpawnBlight ();
							break;
						case "Select":
							SelectBlight ();
							break;						
						}
					}
				});
			}
		}

		public void DestroyBlight (BlightViewModel selectedBlight)
		{
			RemoveBlightViewModel (selectedBlight);

			Task.Run (() => {				
				_parent.BlightService.DestroyBlight (Location, selectedBlight.Blight, Application.CurrentGame);
				Application.SaveCurrentGame ();
			});
		}

		public void MoveBlight (BlightViewModel blightToMove, string newLocation)
		{
			if (Location.Name == newLocation)
				return;

			RemoveBlightViewModel (blightToMove);
			var newLocationSection = _parent.LocationSections.Single (l => l.Location.Name == newLocation);
			newLocationSection.AddBlightViewModel (blightToMove);

			Task.Run (() => {
				Location.Blights.Remove (blightToMove.Blight);
				newLocationSection.Location.Blights.Add (blightToMove.Blight);
				Application.SaveCurrentGame ();
			});
		}

		public RelayCommand AddBlightCommand {
			get { 
				return new RelayCommand (async () => {
				
					var action = await Application.Navigation.DisplayActionSheet (
						"New Blight", 
						"Cancel", 
						null, 
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
						return Blights.Sum(x=>x.Blight.Weight) < 4;
				});
			}
		}


		public void SpawnBlight ()
		{
			var blight = _parent.BlightService.SpawnBlight (Location, Application.CurrentGame).Item2;

			AddBlightViewModel(new BlightViewModel (blight));

			Task.Run (() => {
				Application.SaveCurrentGame ();
			});
		}

		public async void SelectBlight(){
			var blightOptions = Application.CurrentGame.BlightPool.Select (x => x.Name).Distinct ();
			var option = await Application.Navigation.DisplayActionSheet ("Select Blight", "Cancel", null, blightOptions.ToArray ());
			if (option == "Cancel")
				return;

			var blight = Application.CurrentGame.BlightPool.FirstOrDefault(x=>x.Name == option);

			Application.CurrentGame.BlightPool.Remove(blight);
			Location.Blights.Add(blight);

			AddBlightViewModel(new BlightViewModel (blight));
			
			await Task.Run (() => {
				Application.SaveCurrentGame ();
			});
		}
	}

	public class BlightViewModel : BaseViewModel
	{
		public Blight Blight{ get ; private set; }

		public BlightViewModel (Blight blight)
		{
			Blight = blight;
		}

		public bool IsPlaceHolder{
			get { return Blight == null; }
		}

		public ImageSource Image {
			get { 
				if (IsPlaceHolder)
					return ImageSource.FromFile ("plus");
				
				return ImageSourceUtil.GetBlightImage (Blight.Name); 
			}
		}
	}
}

