using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class HeroesPageModel :BaseViewModel
	{
		public HeroesPageModel()
		{
			Heroes = new ObservableCollection<HeroViewModel>();
			//MessagingCenter.Subscribe<HeroViewModel, HeroDefeatedArgs> (this, "HeroDefeated", OnHeroDefeated);
			MessagingCenter.Subscribe<DarknessPopupPageModel>(this, "DarknessUpdated", DarknessUpdated);
		}

		public override void Init(object initData)
		{
			base.Init(initData);
			foreach (var hero in Application.CurrentGame.Heroes)
			{
				var model = HeroViewModel.Create(hero);
				model.CoreMethods = this.CoreMethods;
				Heroes.Add(model);
			}
		}

		public async void DarknessUpdated(DarknessPopupPageModel sender)
		{
			RaisePropertyChanged("Darkness");
			if (Darkness >= Application.CurrentGame.NextDarknessCardDrawAt)
			{
				//CoreMethods.DisplayAlert("", "Draw new Darkness card", "OK");
				//Application.CurrentGame.LastDarknessCardDrawAt = Application.CurrentGame.NextDarknessCardDrawAt;	
			}
		}

		public ObservableCollection<HeroViewModel> Heroes { get; private set; }

		public Command BlightsCommand
		{
			get
			{
				return new Command(() =>
				{
					CoreMethods.PushPageModel<BlightsPageModel>();
				});
			}
		}

		public Command SearchCommand
		{
			get
			{
				return new Command(async (obj) =>
				{
					await CoreMethods.PushPageModel<SearchPageModel>();
				});
			}
		}

		public int Darkness
		{
			get { return Application.CurrentGame.Darkness; }
		}

		public Command DarknessCommand
		{
			get
			{
				return new Command(async (obj) =>
				{
					await CoreMethods.PushPopup<DarknessPopupPageModel>();
				});
			}
		}

		public Command NecromancerCommand
		{
			get
			{
				return new Command(async (obj) =>
				{
					await CoreMethods.PushPageModel<NecromancerCardsPageModel>();
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

