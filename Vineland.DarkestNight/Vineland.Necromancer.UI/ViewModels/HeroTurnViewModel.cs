using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Vineland.DarkestNight.UI;
using Vineland.Necromancer.Core;
using Xamarin.Forms;
using XLabs.Ioc;
using System.Runtime.CompilerServices;

namespace Vineland.Necromancer.UI
{
	//TODO: this whole class is super complicated and could probably do with another pass
	public class HeroTurnViewModel : BaseViewModel
	{
		BlightService _blightService;

		public HeroTurnViewModel (BlightService blightService)
		{
			Locations = new ObservableCollection<LocationViewModel> ();
			_blightService = blightService;

			MessagingCenter.Subscribe<HeroViewModel, HeroDefeatedArgs> (this, "HeroDefeated", OnHeroDefeated);
			MessagingCenter.Subscribe<HeroViewModel, Hero> (this, "HeroUpdated", OnHeroUpdated);
			MessagingCenter.Subscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete", OnNecromancerPhaseComplete);
			MessagingCenter.Subscribe<LocationBlightsViewModel, DestroyBlightArgs> (this, "DestroyBlight", DestroyBlight);
			MessagingCenter.Subscribe<LocationBlightsViewModel, Location> (this, "SpawnBlight", SpawnBlight);
			MessagingCenter.Subscribe<LocationBlightsViewModel, MoveBlightArgs> (this, "MoveBlight", MoveBlight);
			MessagingCenter.Subscribe<LocationBlightsViewModel, Location> (this, "SelectBlight", SelectBlight);
			Initialise ();
		}

		public void Initialise(){
			var models = Application.CurrentGame.Locations.Select (l => new LocationViewModel (l, Application.CurrentGame.Heroes.Where(h=>h.LocationId == l.Id).ToList()));
			foreach (var model in models)
				Locations.Add (model);
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete");
			MessagingCenter.Unsubscribe<HeroViewModel,Hero> (this, "HeroDefeated");
			MessagingCenter.Unsubscribe<HeroViewModel, Hero> (this, "HeroUpdated");
			MessagingCenter.Unsubscribe<LocationBlightsViewModel, DestroyBlightArgs> (this, "DestroyBlight");
			MessagingCenter.Unsubscribe<LocationBlightsViewModel, Location> (this, "SpawnBlight");
			MessagingCenter.Unsubscribe<LocationBlightsViewModel, MoveBlightArgs> (this, "MoveBlight");
			MessagingCenter.Unsubscribe<LocationBlightsViewModel, Location> (this, "SelectBlight");
		}



		public int Darkness {
			get { return Application.CurrentGame.Darkness; }
			set {
				Application.CurrentGame.Darkness = value;
				RaisePropertyChanged (() => Darkness);
			}
		}

		public ObservableCollection<LocationViewModel> Locations { get; private set; }

		public void OnNecromancerPhaseComplete(NecromancerActivationViewModel sender)
		{
			foreach (var spawnLocation in sender.Locations) {
				var location = Locations.Single (l => l.Location.Id == spawnLocation.Location.Id);
				foreach (var spawn in spawnLocation.Spawns) {
					if (spawn is BlightViewModel)
						(location.Single (x => x is LocationBlightsViewModel) as LocationBlightsViewModel).AddBlightViewModel (spawn as BlightViewModel);
				}
			}
			
			RaisePropertyChanged (() => Darkness);
		}

		#region Hero Functions
		public void HeroSelected(Hero hero){
			Application.Navigation.Push<HeroPage>(HeroViewModel.Create(hero));
		}

		private void OnHeroDefeated (HeroViewModel sender, HeroDefeatedArgs args)
		{
			foreach (var location in Locations) {
				var model = location.FirstOrDefault(x => x is HeroSummaryViewModel && (x as HeroSummaryViewModel).Hero.Id == args.DefeatedHero.Id);
				if (model != null)
					location.Remove (model);
			}

			args.NewHero.Grace = args.NewHero.GraceDefault;
			args.NewHero.Secrecy = args.NewHero.SecrecyDefault;

			//first is monastery
			Locations.First().Add (new HeroSummaryViewModel (args.NewHero));

			Task.Run (() => {
				Application.CurrentGame.Heroes.Remove(args.DefeatedHero);
				Application.CurrentGame.Heroes.Add(args.NewHero);
				Application.SaveCurrentGame ();
			});
		}

		private void OnHeroUpdated(HeroViewModel sender, Hero hero){
			foreach (var location in Locations) {
				var model = location.FirstOrDefault(x => x is HeroSummaryViewModel && (x as HeroSummaryViewModel).Hero.Id == hero.Id) as HeroSummaryViewModel;
				if (model != null) {
					model.Updated ();
					if (model.Location != location.Location.Name) {
						location.Remove (model);
						Locations.Single (x => x.Location.Name == model.Location).Add (model);
					}
				}
			}
		}
		#endregion

		#region Blight Functions
		public void DestroyBlight(LocationBlightsViewModel sender, DestroyBlightArgs args){

			sender.RemoveBlightViewModel (args.BlightViewModel);

			Task.Run (() => {				
				_blightService.DestroyBlight (args.Location, args.BlightViewModel.Blight, Application.CurrentGame);
				Application.SaveCurrentGame ();
			});
		}


		public void SpawnBlight (LocationBlightsViewModel sender, Location location)
		{
			var blight = _blightService.SpawnBlight (location, Application.CurrentGame).Item2;

			sender.AddBlightViewModel(new BlightViewModel (blight));

			Task.Run (() => {
				Application.SaveCurrentGame ();
			});
		}



		public void MoveBlight (LocationBlightsViewModel sender, MoveBlightArgs args)
		{
			sender.RemoveBlightViewModel (args.BlightViewModel);
			var newLocationSection = Locations.Single (l => l.Location.Name == args.NewLocationName);
			(newLocationSection.First(x => x is LocationBlightsViewModel) as LocationBlightsViewModel).AddBlightViewModel (args.BlightViewModel);
			
			Task.Run (() => {
				args.CurrentLocation.Blights.Remove (args.BlightViewModel.Blight);
				newLocationSection.Location.Blights.Add (args.BlightViewModel.Blight);
				Application.SaveCurrentGame ();
			});
		}

		public async void SelectBlight(LocationBlightsViewModel sender, Location location){
			var blightOptions = Application.CurrentGame.BlightPool.Select (x => x.Name).Distinct ();
			var option = await Application.Navigation.DisplayActionSheet ("Select Blight", "Cancel", null, blightOptions.ToArray ());
			if (option == "Cancel")
				return;

			var blight = Application.CurrentGame.BlightPool.FirstOrDefault(x=>x.Name == option);

			Application.CurrentGame.BlightPool.Remove(blight);
			location.Blights.Add(blight);

			sender.AddBlightViewModel(new BlightViewModel (blight));

			await Task.Run (() => {
				Application.SaveCurrentGame ();
			});
		}
		#endregion
		public RelayCommand NextPhase {
			get {
				return new RelayCommand (async () => {
					await Application.Navigation.Push<NecromancerPhasePage> ();
					Application.SaveCurrentGame ();
				});
			}
		}

		public class LocationViewModel : ObservableCollection<BaseViewModel>
		{
			public Location Location { get; private set; }

			public LocationViewModel (Location location, List<Hero> heroes)
			{
				Location = location;
				Add(new LocationBlightsViewModel(location));
				foreach(var hero in heroes)
					Add(new HeroSummaryViewModel(hero));
			}
		}

		public class LocationBlightsViewModel :BaseViewModel
		{
			Location _location;

			public LocationBlightsViewModel (Location location)
			{
				_location = location;
				Blights = new ObservableCollection<BlightViewModel> ();
				Blights.Add (new BlightViewModel (null));
				foreach (var blight in location.Blights)
					AddBlightViewModel (new BlightViewModel (blight));
			}

			public ObservableCollection<BlightViewModel> Blights { get; private set; }

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
						if (!blightViewModel.IsPlaceHolder) {
							var action = await Application.Navigation.DisplayActionSheet ("Remove Blight", "Cancel", null, "Destroy", "Move");
							if (action == "Destroy")
								MessagingCenter.Send<LocationBlightsViewModel, DestroyBlightArgs>(this, 
									"DestroyBlight",
									new DestroyBlightArgs(){
										BlightViewModel = blightViewModel,
										Location = _location
									});

							if (action == "Move") {
								var newLocationName = await Application.Navigation.DisplayActionSheet ("New Location", "Cancel", null, Application.CurrentGame.Locations.Select (x => x.Name).ToArray ());
								if (newLocationName != "Cancel"
									&& newLocationName != _location.Name) {
									MessagingCenter.Send<LocationBlightsViewModel, MoveBlightArgs>(this, "MoveBlight",
										new MoveBlightArgs(){
											BlightViewModel = blightViewModel,
											CurrentLocation = _location,
											NewLocationName = newLocationName
										});
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
								MessagingCenter.Send<LocationBlightsViewModel, Location>(this, "SpawnBlight", _location);
								break;
							case "Select":
								MessagingCenter.Send<LocationBlightsViewModel, Location>(this, "SelectBlight", _location);
								break;						
							}
						}
					});
				}
			}


		} 
	}

				public class DestroyBlightArgs{
					public BlightViewModel BlightViewModel;
					public Location Location;
				}

	public class MoveBlightArgs{
		public BlightViewModel BlightViewModel;
		public Location CurrentLocation;
		public string NewLocationName;
	}
}



