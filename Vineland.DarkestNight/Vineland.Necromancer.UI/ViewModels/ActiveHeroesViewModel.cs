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
			HeroRows = new ObservableCollection<HeroSummaryViewModel> (Application.CurrentGame.Heroes.Select (x => new HeroSummaryViewModel (x)).ToList());

			MessagingCenter.Subscribe<HeroViewModel, HeroDefeatedArgs> (this, "HeroDefeated", OnHeroDefeated);
			MessagingCenter.Subscribe<HeroViewModel, Hero> (this, "HeroUpdated", OnHeroUpdated);
			MessagingCenter.Subscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete", OnNecromancerPhaseComplete);
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete");
			MessagingCenter.Unsubscribe<HeroViewModel,Hero> (this, "HeroDefeated");
			MessagingCenter.Unsubscribe<HeroViewModel, Hero> (this, "HeroUpdated");
		}


		public void OnNecromancerPhaseComplete(NecromancerActivationViewModel sender)
		{
			//RaisePropertyChanged (() => Darkness);
		}

		public ObservableCollection<HeroSummaryViewModel> HeroRows { get; private set; }

		public HeroSummaryViewModel SelectedRow { 
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

			HeroRows.Add (new HeroSummaryViewModel (args.NewHero));

			Task.Run (() => {
				Application.CurrentGame.Heroes = HeroRows.Select(x=> x.Hero).ToList ();
				Application.SaveCurrentGame ();
			});
		}

		private void OnHeroUpdated(HeroViewModel sender, Hero hero){
			HeroRows.SingleOrDefault (x => x.Hero.Id == hero.Id).Updated ();
		}
	}
}


