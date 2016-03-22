using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using System.Linq;

namespace Vineland.Necromancer.UI
{
	public class ActiveHeroesViewModel : BaseViewModel
	{

		public ActiveHeroesViewModel ()
		{			
			HeroRows = new ObservableCollection<HeroRowModel> (Application.CurrentGame.Heroes.Select (x => new HeroRowModel (x)).ToList());

			MessagingCenter.Subscribe<HeroViewModel, HeroDefeatedArgs> (this, "HeroDefeated", OnHeroDefeated);
			MessagingCenter.Subscribe<HeroViewModel, Hero> (this, "HeroUpdated", OnHeroUpdated);
			MessagingCenter.Subscribe<NecromancerSpawnViewModel>(this, "NecromancerPhaseComplete", OnNecromancerPhaseComplete);
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<NecromancerSpawnViewModel>(this, "NecromancerPhaseComplete");
			MessagingCenter.Unsubscribe<HeroViewModel,Hero> (this, "HeroDefeated");
			MessagingCenter.Unsubscribe<HeroViewModel, Hero> (this, "HeroUpdated");
		}


		public void OnNecromancerPhaseComplete(NecromancerSpawnViewModel sender)
		{
			RaisePropertyChanged (() => Darkness);
		}

		public ObservableCollection<HeroRowModel> HeroRows { get; private set; }

		public HeroRowModel SelectedRow { 
			get { return null; }
			set{
				if(value != null)
					Application.Navigation.Push<HeroPage>(HeroViewModel.Create(value.Hero));
			}
		}

		private void OnHeroDefeated (HeroViewModel sender, HeroDefeatedArgs args)
		{
			var defeatedHeroRow = HeroRows.SingleOrDefault (x => x.Hero.Id == args.DefeatedHero.Id);
			if (defeatedHeroRow != null)
				HeroRows.Remove (defeatedHeroRow);

			args.NewHero.Grace = args.NewHero.GraceDefault;
			args.NewHero.Secrecy = args.NewHero.SecrecyDefault;

			HeroRows.Add (new HeroRowModel (args.NewHero));

			Task.Run (() => {
				Application.CurrentGame.Heroes = HeroRows.Select(x=> x.Hero).ToList ();
				Application.SaveCurrentGame ();
			});
		}

		private void OnHeroUpdated(HeroViewModel sender, Hero hero){
			HeroRows.SingleOrDefault (x => x.Hero.Id == hero.Id).Updated ();
		}

				public int Darkness {
					get { return Application.CurrentGame.Darkness; }
					set {
						Application.CurrentGame.Darkness = value;
						RaisePropertyChanged (() => Darkness);
					}
				}

		public RelayCommand NextPhase {
			get {
				return new RelayCommand (() => {
					Application.SaveCurrentGame ();
					Application.Navigation.Push<NecromancerPhasePage> ();
				});
			}
		}
	}

	public class HeroRowModel: BaseViewModel
	{
		public Hero Hero;

		public HeroRowModel (Hero hero)
		{
			Hero = hero;
		}

		public ImageSource Image {
			get { return ImageSource.FromFile (Hero.Name.Replace (" ", string.Empty).ToLower ()); }
		}

		public string Name { get { return Hero.Name.ToUpper (); } }

		public string Location { 
			get { 
				var location = Application.CurrentGame.Locations.Single (l => l.Id == Hero.LocationId);
				return location.Name;
			}
		}

		public int Secrecy { get { return Hero.Secrecy; } }

		public int Grace { get { return Hero.Grace; } }

		public void Updated()
		{
			RaisePropertyChanged (() => Secrecy);
			RaisePropertyChanged (() => Grace);
			RaisePropertyChanged (() => Location);
		}
	}
}


