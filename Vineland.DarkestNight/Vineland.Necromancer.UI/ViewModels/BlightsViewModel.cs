using System;
using Vineland.Necromancer.Core;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class BlightsViewModel: BaseViewModel
	{
		BlightService _blightService;

		public ObservableCollection<HeroPhaseLocationViewModel> Locations { get; private set; }

		public BlightsViewModel (BlightService blightService)
		{
			Locations = new ObservableCollection<HeroPhaseLocationViewModel> ();
			SelectedBlights = new List<BlightViewModel>();
			_blightService = blightService;

			MessagingCenter.Subscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete", OnNecromancerPhaseComplete);
			//MessagingCenter.Subscribe<HeroPhaseLocationViewModel, BlightViewModel> (this, "DestroyBlight", DestroyBlight);
			//MessagingCenter.Subscribe<HeroPhaseLocationViewModel> (this, "SpawnBlight", SpawnBlight);
			//MessagingCenter.Subscribe<HeroPhaseLocationViewModel, MoveBlightArgs> (this, "MoveBlight", MoveBlight);
			MessagingCenter.Subscribe<HeroPhaseLocationViewModel, BlightViewModel> (this, "BlightSelected", BlightSelected);

			var locationViewModels = Application.CurrentGame.Locations.Select (l => new HeroPhaseLocationViewModel (l));
			Locations = new ObservableCollection<HeroPhaseLocationViewModel> (locationViewModels);
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel> (this, "DestroyBlight");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel> (this, "SpawnBlight");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel, MoveBlightArgs> (this, "MoveBlight");
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

		public void DestroyBlight(HeroPhaseLocationViewModel sender, BlightViewModel args){
			
				_blightService.DestroyBlight (sender.Location, args.Blight, Application.CurrentGame);
			Application.SaveCurrentGame ();

			sender.RemoveBlightViewModel (args);
		}


		public async void SpawnBlight (HeroPhaseLocationViewModel sender)
		{
			var result = _blightService.SpawnBlight (sender.Location.Id, Application.CurrentGame);
				Application.SaveCurrentGame ();


			sender.AddBlightViewModel(new BlightViewModel (result.Blight));
		}



		public void MoveBlight(HeroPhaseLocationViewModel sender, MoveBlightArgs args)
		{
			var newLocationSection = Locations.Single(l => l.Location.Name == args.NewLocationName);
			if (newLocationSection == sender)
				return;

			//remove from gamestate
			sender.Location.Blights.Remove(args.BlightViewModel.Blight);
			newLocationSection.Location.Blights.Add(args.BlightViewModel.Blight);
			Application.SaveCurrentGame();
			//update view
			sender.RemoveBlightViewModel(args.BlightViewModel);
			newLocationSection.AddBlightViewModel(args.BlightViewModel);
		}

		public List<BlightViewModel> SelectedBlights { get; private set; }

		public async void BlightSelected(HeroPhaseLocationViewModel sender, BlightViewModel blight)
		{
			if (blight.IsSelected)
				SelectedBlights.Add(blight);
			else
				SelectedBlights.Remove(blight);
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
		public HeroPhaseLocationViewModel(Location location)
				: base(location)
		{
			Spawns.Add(new BlightViewModel(null));
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
					if (blightViewModel.IsPlaceHolder)
					{
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
					else {
						blightViewModel.IsSelected = !blightViewModel.IsSelected;
						MessagingCenter.Send<HeroPhaseLocationViewModel, BlightViewModel>(this, "BlightSelected", blightViewModel);
					}
					//if (!blightViewModel.IsPlaceHolder)
					//{
					//	var action = await Application.Navigation.DisplayActionSheet("Remove Blight", "Cancel", null, "Destroy", "Move");
					//	if (action == "Destroy")
					//		MessagingCenter.Send<HeroPhaseLocationViewModel, BlightViewModel>(this,
					//			"DestroyBlight",
					//			blightViewModel
					//		);

					//	if (action == "Move")
					//	{
					//		var newLocationName = await Application.Navigation.DisplayActionSheet("New Location", "Cancel", null, Application.CurrentGame.Locations.Select(x => x.Name).ToArray());
					//		if (newLocationName != "Cancel"
					//			&& newLocationName != Location.Name)
					//		{
					//			MessagingCenter.Send<HeroPhaseLocationViewModel, MoveBlightArgs>(this, "MoveBlight",
					//				new MoveBlightArgs()
					//				{
					//					BlightViewModel = blightViewModel,
					//					NewLocationName = newLocationName
					//				});
					//		}
					//	}
					//}
					//else {
					//	var action = await Application.Navigation.DisplayActionSheet(
					//		"New Blight",
					//		"Cancel",
					//		null,
					//		"Spawn",
					//		"Select");
					//	switch (action)
					//	{
					//		case "Spawn":
					//			MessagingCenter.Send<HeroPhaseLocationViewModel>(this, "SpawnBlight");
					//			break;
					//		case "Select":
					//			MessagingCenter.Send<HeroPhaseLocationViewModel>(this, "SelectBlight");
					//			break;
					//	}
					//}
				});
			}
		}
	}

	public class MoveBlightArgs{
		public BlightViewModel BlightViewModel;
		public string NewLocationName;
	}
}

