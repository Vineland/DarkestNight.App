using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using System.Linq;
using Android.Util;

namespace Vineland.Necromancer.UI
{
	public class ActiveHeroesViewModel : BaseViewModel
	{

		public ActiveHeroesViewModel ()
		{			
			Heroes = new ObservableCollection<HeroRowViewModel> (Application.CurrentGame.Heroes.Select (x => new HeroRowViewModel (x)).ToList());
			//MessagingCenter.Subscribe<SelectHeroViewModel,Hero> (this, "HeroSelected", OnHeroSelected);
		}

		public override void Cleanup ()
		{
			base.Cleanup ();
			//MessagingCenter.Unsubscribe<SelectHeroViewModel,Hero> (this, "HeroSelected");
		}

		public ObservableCollection<HeroRowViewModel> Heroes { get; private set; }

		public HeroRowViewModel SelectedRow { 
			get { return null; }
			set{
				if(value != null)
					Application.Navigation.Push<HeroTurnPage>(new HeroTurnViewModel(value.Hero));
			}
		}
		//		HeroRowViewModel _heroRowToRemove;
		//		public RelayCommand<HeroRowViewModel> RemoveHero {
		//			get {
		//				return new RelayCommand<HeroRowViewModel> (
		//					(heroRow) => {
		//						_heroToRemove = heroRow;
		//						Application.Navigation.Push<SelectHeroPage> ();
		//					});
		//			}
		//		}
		//
		//		private void OnHeroSelected (SelectHeroViewModel sender, Hero selectedHero)
		//		{
		//			//no hero was selected i.e they backed out or cancelled
		//			if (selectedHero == null)
		//				return;
		//
		//			//remove current hero
		//			var index = Heroes.IndexOf (_heroToRemove);
		//			Heroes.Remove (_heroToRemove);
		//			Heroes.Insert (index, selectedHero);
		//			Task.Run (() => {
		//				Application.CurrentGame.Heroes = Heroes.ToList ();
		//				Application.SaveCurrentGame ();
		//			});
		//		}

		//		public int Darkness {
		//			get { return Application.CurrentGame.Darkness; }
		//			set {
		//				Application.CurrentGame.Darkness = value;
		//				RaisePropertyChanged (() => Darkness);
		//			}
		//		}

		public RelayCommand NextPhase {
			get {
				return new RelayCommand (() => {
					Application.SaveCurrentGame ();
					Application.Navigation.Push<NecromancerPhasePage> ();
				});
			}
		}
	}

	public class HeroRowViewModel: BaseViewModel
	{
		public Hero Hero;

		public HeroRowViewModel (Hero hero)
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
	}
}


