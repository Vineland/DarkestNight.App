using System;
using Vineland.Necromancer.Core;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class BlightsViewModel: BaseViewModel
	{
		BlightService _blightService;

		public ObservableCollection<HeroPhaseLocationViewModel> Locations { get; private set; }

		public BlightsViewModel (BlightService blightService)
		{
			Locations = new ObservableCollection<HeroPhaseLocationViewModel> ();

			_blightService = blightService;

			MessagingCenter.Subscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete", OnNecromancerPhaseComplete);
			MessagingCenter.Subscribe<BlightViewModel> (this, "DestroyBlight", DestroyBlight);
			MessagingCenter.Subscribe<HeroPhaseLocationViewModel> (this, "SpawnBlight", SpawnBlight);
			MessagingCenter.Subscribe<BlightViewModel> (this, "MoveBlight", MoveBlight);
			MessagingCenter.Subscribe<AddBlightModel> (this, "AddBlight", AddBlight);

			var locationViewModels = Application.CurrentGame.Locations.Select (l => new HeroPhaseLocationViewModel (l, Application.CurrentGame.Heroes.Active.Where(x=>x.LocationId == l.Id).ToList()));
			Locations = new ObservableCollection<HeroPhaseLocationViewModel> (locationViewModels);
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete");
			MessagingCenter.Unsubscribe<BlightViewModel> (this, "DestroyBlight");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel> (this, "SpawnBlight");
			MessagingCenter.Unsubscribe<BlightViewModel> (this, "MoveBlight");
			MessagingCenter.Unsubscribe<AddBlightModel> (this, "AddBlight");
		}

		public void OnNecromancerPhaseComplete(NecromancerActivationViewModel sender)
		{
			foreach (var spawnLocation in sender.Locations) {
				var locationRow = Locations.Single (l => l.Location.Id == spawnLocation.Location.Id);
				foreach (var spawn in spawnLocation.Spawns.Where(x=> x is BlightViewModel))
					locationRow.AddBlightViewModel (spawn as BlightViewModel);
			}
		}

		public async void DestroyBlight(BlightViewModel sender){

			if (await Application.Navigation.DisplayConfirmation("Destroy Blight", "Are you sure you wish to remove this blight?", "Yes", "No"))
			{
				var locationModel = Locations.First(x => x.Spawns.Contains(sender));
				_blightService.DestroyBlight(locationModel.Location, sender.Blight, Application.CurrentGame);
				Application.SaveCurrentGame();

				locationModel.RemoveBlightViewModel(sender);
			}
		}

		public async void AddBlight(AddBlightModel sender)
		{
			var action = await Application.Navigation.DisplayActionSheet(
							"New Blight",
							"Cancel",
							null,
							"Spawn",
							"Select");
			if (action != "Cancel")
			{
				var locationSection = Locations.First(x => x.Spawns.Contains(sender));
				switch (action)
				{
					case "Spawn":
						SpawnBlight(locationSection);
						break;
					case "Select":
						SelectBlight(locationSection);
						break;
				}
			}
		}

		public void SpawnBlight(HeroPhaseLocationViewModel locationSection)
		{
			Blight blight = null;

			blight = _blightService.SpawnBlight(locationSection.Location, Application.CurrentGame).Item2;
			Application.SaveCurrentGame();

			locationSection.AddBlightViewModel(new BlightViewModel(blight));
		}



		public async void MoveBlight(BlightViewModel sender)
		{
			var newLocationName = await Application.Navigation.DisplayActionSheet("New Location", "Cancel", null, Application.CurrentGame.Locations.Select(x => x.Name).ToArray());
			if (newLocationName != "Cancel")
			{
				var currentLocationSection = Locations.First(x => x.Spawns.Contains(sender));
				var newLocationSection = Locations.Single(l => l.Location.Name == newLocationName);
				if (currentLocationSection == newLocationSection)
					return;

				//remove from gamestate
				currentLocationSection.Location.Blights.Remove(sender.Blight);
				newLocationSection.Location.Blights.Add(sender.Blight);
				Application.SaveCurrentGame();
				//update view
				currentLocationSection.RemoveBlightViewModel(sender);
				newLocationSection.AddBlightViewModel(sender);
			}
		}

		public async void SelectBlight(HeroPhaseLocationViewModel locationSection)
		{
			var blightOptions = Application.CurrentGame.BlightPool.Select(x => x.Name).Distinct();
			var option = await Application.Navigation.DisplayActionSheet("Select Blight", "Cancel", null, blightOptions.ToArray());
			if (option != "Cancel")
			{
				var blight = Application.CurrentGame.BlightPool.FirstOrDefault(x => x.Name == option);

				Application.CurrentGame.BlightPool.Remove(blight);
				locationSection.Location.Blights.Add(blight);

				locationSection.AddBlightViewModel(new BlightViewModel(blight));
				Application.SaveCurrentGame();
			}
		}
	}

	public class HeroPhaseLocationViewModel : LocationViewModel
	{
		public ObservableCollection<HeroSummaryViewModel> Heroes { get; private set;}

		public HeroPhaseLocationViewModel(Location location, IEnumerable<Hero> heroes)
				: base(location)
		{
			Heroes = new ObservableCollection<HeroSummaryViewModel>(heroes.Select(x => new HeroSummaryViewModel(x)));
			CheckAddBlightOption();
		}

		public void AddBlightViewModel(BlightViewModel viewModel)
		{
			if (Spawns.Any() && Spawns.Last() is AddBlightModel)
				Spawns.Insert(Spawns.Count - 1, viewModel);
			else
				Spawns.Add(viewModel);

			CheckAddBlightOption();
		}

		public void RemoveBlightViewModel(BlightViewModel viewModel)
		{
			if (!Spawns.Contains(viewModel))
				return;

			Spawns.Remove(viewModel);

			var blights = Spawns.Where(x => x is BlightViewModel).Select(x => x as BlightViewModel).ToList();

			CheckAddBlightOption();
		}

		private void CheckAddBlightOption()
		{
			if (Location.Blights.Sum(x => x.Weight) >= 4
				&& Spawns.Last() is AddBlightModel)
				Spawns.RemoveAt(Spawns.Count - 1);
			else if (Location.Blights.Sum(x => x.Weight) < 4
			         &&  (!Spawns.Any() || !(Spawns.Last() is AddBlightModel)))
				Spawns.Add(new AddBlightModel());
		}
	}
}

