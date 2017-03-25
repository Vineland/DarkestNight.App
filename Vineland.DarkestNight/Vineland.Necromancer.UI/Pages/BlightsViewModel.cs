using System;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.Core.Models;
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

			MessagingCenter.Subscribe<HeroPhaseLocationViewModel, BlightViewModel> (this, "BlightSelected", BlightSelected);
			MessagingCenter.Subscribe<HeroPhaseLocationViewModel>(this, "SpawnBlight", SpawnBlight);

			var locationViewModels = Application.CurrentGame.Locations.Select (l => new HeroPhaseLocationViewModel (l));
			Locations = new ObservableCollection<HeroPhaseLocationViewModel> (locationViewModels);
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel, BlightViewModel> (this, "BlightSelected");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel>(this, "SpawnBlight");
		}

		public void OnNecromancerPhaseComplete(NecromancerActivationViewModel sender)
		{
			foreach (var spawnLocation in sender.Locations) {
				var locationRow = Locations.Single (l => l.Location.Id == spawnLocation.Location.Id);
				foreach (var spawn in spawnLocation.Spawns.Where(x=> x is BlightViewModel))
					locationRow.AddBlightViewModel (spawn as BlightViewModel);
			}
		}

		public void SpawnBlight(HeroPhaseLocationViewModel locationModel)
		{
			var viewModel = new SpawnBlightPopupViewModel();
			viewModel.OnOptionSelected += (SpawnBlightOption option) =>
			{
				SpawnBlightResult result;
				if (option.Description == "Random")
					result = _blightService.SpawnBlight(locationModel.Location.Id, Application.CurrentGame);
				else
					result = _blightService.SpawnBlight(locationModel.Location.Id, option.Description, Application.CurrentGame);

				locationModel.AddBlightViewModel(new BlightViewModel(result.Blight));
			};

			Application.Navigation.PushPopup<SpawnBlightPopupPage>(viewModel);
		}

		public List<BlightViewModel> SelectedBlights { get; private set; }

		public void BlightSelected(HeroPhaseLocationViewModel sender, BlightViewModel blight)
		{
			if (blight.IsSelected)
				SelectedBlights.Add(blight);
			else
				SelectedBlights.Remove(blight);

			RaisePropertyChanged(() => MoveCommand);
			RaisePropertyChanged(() => DestroyCommand);
		}

		public RelayCommand DestroyCommand
		{
			get
			{
				return new RelayCommand(async () =>
				{
					string message;
					if (SelectedBlights.Count == 1)
						message = "Destroy the selected blight?";
					else
						message = $"Destroy the {SelectedBlights.Count} selected blights?";

					var result = await Application.Navigation.DisplayConfirmation("Destroy", message, "Yes", "No");
					if (result)
						DestroyBlights();
				}, () => { return SelectedBlights.Any(); });
			}
		}

		public RelayCommand MoveCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					var viewModel = new ChooseLocationPopupViewModel();
					viewModel.OnLocationSelected += MoveSelectedBlights;
					Application.Navigation.PushPopup<ChooseLocationPopupPage>(viewModel);
				}, () => { return SelectedBlights.Any(); });
			}
		}

		void MoveSelectedBlights(Location location)
		{
			var newLocation = Locations.Single(l => l.Location.Id == location.Id);

			foreach (var blightModel in SelectedBlights)
			{
				var currentLocation = Locations.First(x => x.Spawns.Contains(blightModel));
				if (currentLocation == newLocation)
					continue;

				//remove from gamestate
				newLocation.Location.Blights.Add(blightModel.Blight);
				currentLocation.Location.Blights.Remove(blightModel.Blight);
				//update view
				currentLocation.RemoveBlightViewModel(blightModel);
				newLocation.AddBlightViewModel(blightModel);
			}

			Application.SaveCurrentGame();
			SelectedBlights.ForEach(x => x.IsSelected = false);
			SelectedBlights.Clear();
		}

		public void DestroyBlights()
		{
			foreach (var blightModel in SelectedBlights)
			{
				var locationModel = Locations.First(l => l.Spawns.Contains(blightModel));
				_blightService.DestroyBlight(locationModel.Location, blightModel.Blight, Application.CurrentGame);
				locationModel.RemoveBlightViewModel(blightModel);
			}
			SelectedBlights.Clear();
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

			Location.Blights.Remove(viewModel.Blight);
			Spawns.Remove(viewModel);

			if (Location.Blights.Sum(x => x.Weight) < 4
			    && !Spawns.Any(x=> x.IsPlaceHolder))
				Spawns.Add(new BlightViewModel(null));
		}

		public override RelayCommand<BlightViewModel> BlightSelectedCommand
		{
			get
			{
				return new RelayCommand<BlightViewModel>((blightViewModel) =>
				{
					if (blightViewModel.IsPlaceHolder)
					{
						MessagingCenter.Send<HeroPhaseLocationViewModel>(this, "SpawnBlight");
					}
					else {
						blightViewModel.IsSelected = !blightViewModel.IsSelected;
						MessagingCenter.Send<HeroPhaseLocationViewModel, BlightViewModel>(this, "BlightSelected", blightViewModel);
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

