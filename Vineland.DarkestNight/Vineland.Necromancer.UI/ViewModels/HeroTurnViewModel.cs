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
			Rows = new ObservableCollection<BaseViewModel> ();
			_blightService = blightService;

			MessagingCenter.Subscribe<HeroViewModel, HeroDefeatedArgs> (this, "HeroDefeated", OnHeroDefeated);
			MessagingCenter.Subscribe<HeroViewModel, Hero> (this, "HeroUpdated", OnHeroUpdated);
			MessagingCenter.Subscribe<HeroViewModel, Hero> (this, "HeroMoved", OnHeroMoved);
			MessagingCenter.Subscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete", OnNecromancerPhaseComplete);
			MessagingCenter.Subscribe<HeroPhaseLocationViewModel, BlightViewModel> (this, "DestroyBlight", DestroyBlight);
			MessagingCenter.Subscribe<HeroPhaseLocationViewModel> (this, "SpawnBlight", SpawnBlight);
			MessagingCenter.Subscribe<HeroPhaseLocationViewModel, MoveBlightArgs> (this, "MoveBlight", MoveBlight);
			MessagingCenter.Subscribe<HeroPhaseLocationViewModel> (this, "SelectBlight", SelectBlight);
			Initialise ();
		}

		public void Initialise(){
			foreach (var location in Application.CurrentGame.Locations) 
			{
				Rows.Add (new HeroPhaseLocationViewModel (location));
				foreach (var hero in Application.CurrentGame.Heroes.Where(x=>x.LocationId == location.Id))
					Rows.Add (new HeroSummaryViewModel (hero));
			}
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete");
			MessagingCenter.Unsubscribe<HeroViewModel,Hero> (this, "HeroDefeated");
			MessagingCenter.Unsubscribe<HeroViewModel, Hero> (this, "HeroUpdated");
			MessagingCenter.Unsubscribe<HeroViewModel, Hero> (this, "HeroMoved");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel, BlightViewModel> (this, "DestroyBlight");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel> (this, "SpawnBlight");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel, MoveBlightArgs> (this, "MoveBlight");
			MessagingCenter.Unsubscribe<HeroPhaseLocationViewModel> (this, "SelectBlight");
		}



		public int Darkness {
			get { return Application.CurrentGame.Darkness; }
			set {
				Application.CurrentGame.Darkness = value;
				RaisePropertyChanged (() => Darkness);
			}
		}

		public ObservableCollection<BaseViewModel> Rows { get; private set; }

		private List<HeroPhaseLocationViewModel> LocationRows {
			get{ return Rows.Where (x => x is HeroPhaseLocationViewModel).Select(x=> x as HeroPhaseLocationViewModel).ToList(); }
		}

		private List<HeroSummaryViewModel> HeroRows {
			get{ return Rows.Where (x => x is HeroSummaryViewModel).Select(x=> x as HeroSummaryViewModel).ToList(); }
		}

		public void OnNecromancerPhaseComplete(NecromancerActivationViewModel sender)
		{
			var locationRows = LocationRows;
			foreach (var spawnLocation in sender.Locations) {
				var locationRow = locationRows.Single (l => l.Location.Id == spawnLocation.Location.Id);
				foreach (var spawn in spawnLocation.Spawns.Where(x=> x is BlightViewModel))
					locationRow.AddBlightViewModel (spawn as BlightViewModel);
			}
			
			RaisePropertyChanged (() => Darkness);
		}

		#region Hero Functions
		public void HeroSelected(Hero hero){
			Application.Navigation.Push<HeroPage>(HeroViewModel.Create(hero));
		}

		private void OnHeroDefeated (HeroViewModel sender, HeroDefeatedArgs args)
		{
			var model = HeroRows.FirstOrDefault (x => x.Hero.Id == args.DefeatedHero.Id);
			if (model != null)
				Rows.Remove (model);

			args.NewHero.Grace = args.NewHero.GraceDefault;
			args.NewHero.Secrecy = args.NewHero.SecrecyDefault;

			//first (index 0) is the monastery
			Rows.Insert(1, new HeroSummaryViewModel (args.NewHero));

			Task.Run (() => {
				Application.CurrentGame.Heroes.Remove (args.DefeatedHero);
				Application.CurrentGame.Heroes.Add (args.NewHero);
				Application.SaveCurrentGame ();
			});
		}

		private void OnHeroUpdated(HeroViewModel sender, Hero hero){
			var heroRow = HeroRows.Single (x => x.Hero.Id == hero.Id);
			heroRow.Updated ();
		}

		private void OnHeroMoved(HeroViewModel sender, Hero hero){
			var heroRow = HeroRows.Single (x => x.Hero.Id == hero.Id);
			//yipes!
			Rows.Remove(heroRow);
			var indexOfLocation = Rows.IndexOf (LocationRows.Single (l => l.Location.Id == hero.LocationId));
			Rows.Insert (indexOfLocation + 1, heroRow);
		}
		#endregion

		#region Blight Functions
		public void DestroyBlight(HeroPhaseLocationViewModel sender, BlightViewModel blightViewModel){

			sender.RemoveBlightViewModel (blightViewModel);

			Task.Run (() => {				
				_blightService.DestroyBlight (sender.Location, blightViewModel.Blight, Application.CurrentGame);
				Application.SaveCurrentGame ();
			});
		}


		public void SpawnBlight (HeroPhaseLocationViewModel sender)
		{
			var blight = _blightService.SpawnBlight (sender.Location, Application.CurrentGame).Item2;

			sender.AddBlightViewModel(new BlightViewModel (blight));

			Task.Run (() => {
				Application.SaveCurrentGame ();
			});
		}



		public void MoveBlight (HeroPhaseLocationViewModel sender, MoveBlightArgs args)
		{
			sender.RemoveBlightViewModel (args.BlightViewModel);
			var newLocationSection = LocationRows.Single (l => l.Location.Name == args.NewLocationName);
			newLocationSection.AddBlightViewModel (args.BlightViewModel);
			
			Task.Run (() => {
				sender.Location.Blights.Remove (args.BlightViewModel.Blight);
				newLocationSection.Location.Blights.Add (args.BlightViewModel.Blight);
				Application.SaveCurrentGame ();
			});
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
		#endregion
		public RelayCommand NextPhase {
			get {
				return new RelayCommand (async () => {
					await Application.Navigation.Push<NecromancerPhasePage> ();
					Application.SaveCurrentGame ();
				});
			}
		}

		public class HeroPhaseLocationViewModel : LocationViewModel
		{

			public HeroPhaseLocationViewModel (Location location)
				:base(location)
			{
				Spawns.Add(new BlightViewModel(null));
			}

			public void AddBlightViewModel(BlightViewModel viewModel){
				var blights = Spawns.Where (x => x is BlightViewModel).Select(x=>x as BlightViewModel).ToList();
				if  (blights.Any() && blights.Last().IsPlaceHolder)
					Spawns.Insert (Spawns.Count - 1, viewModel);
				else
					Spawns.Add(viewModel);

				if (blights.Where (x => !x.IsPlaceHolder).Sum (x => x.Blight.Weight) >= 4
					&& blights.Last().IsPlaceHolder)
					Spawns.RemoveAt (Spawns.Count - 1);
			}

			public void RemoveBlightViewModel(BlightViewModel viewModel){
				if (!Spawns.Contains (viewModel))
					return;

				Spawns.Remove (viewModel);

				var blights = Spawns.Where (x => x is BlightViewModel).Select(x=>x as BlightViewModel).ToList();

				if (blights.Where (x => !x.IsPlaceHolder).Sum (x => x.Blight.Weight) < 4
					&& !blights.Last().IsPlaceHolder)
					Spawns.Add (new BlightViewModel (null));
			}

			public override RelayCommand<BlightViewModel> BlightSelectedCommand{
				get{
					return new RelayCommand<BlightViewModel> (async (blightViewModel) => {
						if (!blightViewModel.IsPlaceHolder) {
							var action = await Application.Navigation.DisplayActionSheet ("Remove Blight", "Cancel", null, "Destroy", "Move");
							if (action == "Destroy")
								MessagingCenter.Send<HeroPhaseLocationViewModel, BlightViewModel>(this, 
									"DestroyBlight",
									blightViewModel
								);

							if (action == "Move") {
								var newLocationName = await Application.Navigation.DisplayActionSheet ("New Location", "Cancel", null, Application.CurrentGame.Locations.Select (x => x.Name).ToArray ());
								if (newLocationName != "Cancel"
									&& newLocationName != Location.Name) {
									MessagingCenter.Send<HeroPhaseLocationViewModel, MoveBlightArgs>(this, "MoveBlight",
										new MoveBlightArgs(){
											BlightViewModel = blightViewModel,
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
	}

	public class MoveBlightArgs{
		public BlightViewModel BlightViewModel;
		public string NewLocationName;
	}
}



