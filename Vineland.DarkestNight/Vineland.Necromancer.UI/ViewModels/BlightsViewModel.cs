﻿using System;
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
			MessagingCenter.Subscribe<BlightViewModel, MoveBlightArgs> (this, "MoveBlight", MoveBlight);
			MessagingCenter.Subscribe<HeroPhaseLocationViewModel> (this, "SelectBlight", SelectBlight);

			var locationViewModels = Application.CurrentGame.Locations.Select (l => new HeroPhaseLocationViewModel (l, Application.CurrentGame.Heroes.Active.Where(x=>x.LocationId == l.Id).ToList()));
			Locations = new ObservableCollection<HeroPhaseLocationViewModel> (locationViewModels);
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete");
			MessagingCenter.Unsubscribe<BlightViewModel> (this, "DestroyBlight");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel> (this, "SpawnBlight");
			MessagingCenter.Unsubscribe<BlightViewModel, MoveBlightArgs> (this, "MoveBlight");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel> (this, "SelectBlight");
		}

		public void OnNecromancerPhaseComplete(NecromancerActivationViewModel sender)
		{
			foreach (var spawnLocation in sender.Locations) {
				var locationRow = Locations.Single (l => l.Location.Id == spawnLocation.Location.Id);
				foreach (var spawn in spawnLocation.Spawns.Where(x=> x is BlightViewModel))
					locationRow.AddBlightViewModel (spawn as BlightViewModel);
			}
		}

		public void DestroyBlight(BlightViewModel sender){

				var locationModel = Locations.First(x => x.Spawns.Contains(sender));
				_blightService.DestroyBlight (locationModel.Location, sender.Blight, Application.CurrentGame);
				Application.SaveCurrentGame ();

			locationModel.RemoveBlightViewModel (sender);
		}


		public async void SpawnBlight (HeroPhaseLocationViewModel sender)
		{
			Blight blight = null;

			await Task.Run (() => {

				blight = _blightService.SpawnBlight (sender.Location, Application.CurrentGame).Item2;
				Application.SaveCurrentGame ();
			});

			sender.AddBlightViewModel(new BlightViewModel (blight));
		}



		public void MoveBlight(BlightViewModel sender, MoveBlightArgs args)
		{
			var currentLocationSection = Locations.First(x => x.Spawns.Contains(sender));
			var newLocationSection = Locations.Single(l => l.Location.Name == args.NewLocationName);
			if (currentLocationSection == newLocationSection)
				return;

			//remove from gamestate
			currentLocationSection.Location.Blights.Remove(args.BlightViewModel.Blight);
			newLocationSection.Location.Blights.Add(args.BlightViewModel.Blight);
			Application.SaveCurrentGame();
			//update view
			currentLocationSection.RemoveBlightViewModel(args.BlightViewModel);
			newLocationSection.AddBlightViewModel(args.BlightViewModel);
		}

		public async void SelectBlight(HeroPhaseLocationViewModel sender){
			var blightOptions = Application.CurrentGame.BlightPool.Select (x => x.Name).Distinct ();
			var option = await Application.Navigation.DisplayActionSheet ("Select Blight", "Cancel", null, blightOptions.ToArray ());
			if (option == "Cancel")
				return;

			var blight = Application.CurrentGame.BlightPool.FirstOrDefault(x=>x.Name == option);

			Application.CurrentGame.BlightPool.Remove(blight);
			sender.Location.Blights.Add(blight);

			sender.AddBlightViewModel(new BlightViewModel (blight));

			await Task.Run (() => {
				Application.SaveCurrentGame ();
			});
		}
	}

	public class HeroPhaseLocationViewModel : LocationViewModel
	{
		public ObservableCollection<HeroSummaryViewModel> Heroes { get; private set;}

		public HeroPhaseLocationViewModel(Location location, IEnumerable<Hero> heroes)
				: base(location)
		{
			Spawns.Add(new BlightViewModel(null));

			Heroes = new ObservableCollection<HeroSummaryViewModel>(heroes.Select(x => new HeroSummaryViewModel(x)));

		}

		public void AddBlightViewModel(BlightViewModel viewModel)
		{
			var blights = Spawns.Where(x => x is BlightViewModel).Select(x => x as BlightViewModel).ToList();
			if (blights.Any() && blights.Last().IsPlaceHolder)
				Spawns.Insert(Spawns.Count - 1, viewModel);
			else
				Spawns.Add(viewModel);

			if (Location.Blights.Sum(x => x.Weight) >= 4
				&& blights.Last().IsPlaceHolder)
				Spawns.RemoveAt(Spawns.Count - 1);
		}

		public void RemoveBlightViewModel(BlightViewModel viewModel)
		{
			if (!Spawns.Contains(viewModel))
				return;

			Spawns.Remove(viewModel);

			var blights = Spawns.Where(x => x is BlightViewModel).Select(x => x as BlightViewModel).ToList();

			if (Location.Blights.Sum(x => x.Weight) < 4
				&& !blights.Last().IsPlaceHolder)
				Spawns.Add(new BlightViewModel(null));
		}

		public override RelayCommand<BlightViewModel> BlightSelectedCommand
		{
			get
			{
				return new RelayCommand<BlightViewModel>(async (blightViewModel) =>
				{
					if (!blightViewModel.IsPlaceHolder)
					{
						var action = await Application.Navigation.DisplayActionSheet("Remove Blight", "Cancel", null, "Destroy", "Move");
						if (action == "Destroy")
							MessagingCenter.Send<HeroPhaseLocationViewModel, BlightViewModel>(this,
								"DestroyBlight",
								blightViewModel
							);

						if (action == "Move")
						{
							var newLocationName = await Application.Navigation.DisplayActionSheet("New Location", "Cancel", null, Application.CurrentGame.Locations.Select(x => x.Name).ToArray());
							if (newLocationName != "Cancel"
								&& newLocationName != Location.Name)
							{
								MessagingCenter.Send<HeroPhaseLocationViewModel, MoveBlightArgs>(this, "MoveBlight",
									new MoveBlightArgs()
									{
										BlightViewModel = blightViewModel,
										NewLocationName = newLocationName
									});
							}
						}
					}
					else {
						var action = await Application.Navigation.DisplayActionSheet(
							"New Blight",
							"Cancel",
							null,
							"Spawn",
							"Select");
						switch (action)
						{
							case "Spawn":
								MessagingCenter.Send<HeroPhaseLocationViewModel>(this, "SpawnBlight");
								break;
							case "Select":
								MessagingCenter.Send<HeroPhaseLocationViewModel>(this, "SelectBlight");
								break;
						}
					}
				});
			}
		}
	}

	public class MoveBlightArgs{
		public BlightViewModel BlightViewModel;
		public string NewLocationName;
	}
}

