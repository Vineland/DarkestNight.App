using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Vineland.Necromancer.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Vineland.Necromancer.Domain;
using GalaSoft.MvvmLight.Command;

namespace Vineland.Necromancer.UI
{
	public class HeroesViewModel :BaseViewModel
	{
		public HeroesViewModel ()
		{
			Heroes = new ObservableCollection<HeroViewModel> ();

			//MessagingCenter.Subscribe<HeroViewModel, HeroDefeatedArgs> (this, "HeroDefeated", OnHeroDefeated);
			Initialise ();
		}

		public void Initialise(){
			foreach (var hero in Application.CurrentGame.Heroes)
				Heroes.Add (HeroViewModel.Create(hero));
		}

		//public override void Cleanup ()
		//{
		//	base.OnDisappearing ();
		//	MessagingCenter.Unsubscribe<HeroViewModel,Hero> (this, "HeroDefeated");
		//}

		public ObservableCollection<HeroViewModel> Heroes { get; private set; }

		public RelayCommand BlightsCommand
		{
			get
			{
				return new RelayCommand(async () =>
				{
					await Application.Navigation.Push<BlightsPage>();
				});
			}
		}

		public RelayCommand SearchCommand
		{
			get
			{
				return new RelayCommand(async () =>
				{
					await Application.Navigation.Push<SearchPage>();
				});
			}
		}

		public int Darkness
		{
			get { return Application.CurrentGame.Darkness; }
		}

		public RelayCommand DarknessCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					Application.Navigation.PushPopup<DarknessPopupPage>();
				});
			}
		}

		public RelayCommand NecromancerCommand
		{
			get
			{
				return new RelayCommand(async () =>
				{
					await Application.Navigation.Push<NecromancerPhasePage>();
				});
			}
		}

		//public bool AcolytePresent { get { return Application.CurrentGame.Heroes.GetHero<Acolyte> () != null; } }
		//public bool ConjurerPresent { get { return Application.CurrentGame.Heroes.GetHero<Conjurer> () != null; } }
		//public bool ParagonPresent { get { return Application.CurrentGame.Heroes.GetHero<Paragon> () != null; } }
		//public bool RangerPresent { get { return Application.CurrentGame.Heroes.GetHero<Ranger> () != null; } }
		//public bool ScholarPresent { get { return Application.CurrentGame.Heroes.GetHero<Scholar> () != null; } }
		//public bool SeerPresent { get { return Application.CurrentGame.Heroes.GetHero<Seer> () != null; } }
		//public bool ValkyriePresent { get { return Application.CurrentGame.Heroes.GetHero<Valkyrie> () != null; } }
		//public bool WizardPresent { get { return Application.CurrentGame.Heroes.GetHero<Wizard> () != null; } }
		//public bool WayfarerPresent { get { return Application.CurrentGame.Heroes.GetHero<Wayfarer> () != null; } }

		//public HeroesState HeroesState { get { return Application.CurrentGame.Heroes; } }

		//private void OnHeroDefeated (HeroViewModel sender, HeroDefeatedArgs args)
		//{
		//	var model = Heroes.FirstOrDefault (x => x.Id == args.DefeatedHero.Id);
		//	if (model != null)
		//		Heroes.Remove (model);

		//	args.NewHero.Grace = args.NewHero.GraceDefault;
		//	args.NewHero.Secrecy = args.NewHero.SecrecyDefault;

		//	Heroes.Add(new HeroSummaryViewModel (args.NewHero));

		//	Task.Run (() => {
		//		Application.CurrentGame.Heroes.Remove (args.DefeatedHero);
		//		Application.CurrentGame.Heroes.Add (args.NewHero);
		//		Application.SaveCurrentGame ();
		//	});
		//}
	}

}

