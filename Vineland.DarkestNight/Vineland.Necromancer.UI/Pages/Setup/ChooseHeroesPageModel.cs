using System.Collections.ObjectModel;
using System.Linq;
using Vineland.Necromancer.Core;
using Xamarin.Forms;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class ChooseHeroesPageModel : BaseViewModel
	{
		DataService _dataService;

		public ChooseHeroesPageModel(DataService dataService)
		{
			_dataService = dataService;

			HeroSlots = new ObservableCollection<HeroSlotViewModel> ();
			AvailableHeroes = new ObservableCollection<HeroViewModel> ();
		}
		public override void Init(object initData)
		{
			base.Init(initData);
			
			for (int i = 0; i < Application.CurrentGame.NumberOfPlayers; i++) {
				HeroSlots.Add (new HeroSlotViewModel ());
			}
			foreach (var hero in _dataService.GetAllHeroes())
				AvailableHeroes.Add (new HeroViewModel(hero));
		}

		public ObservableCollection<HeroSlotViewModel> HeroSlots { get; set; }

		public ObservableCollection<HeroViewModel> AvailableHeroes { get; set; }

		public void SelectHero(HeroViewModel heroViewModel){
			if (!HeroSlots.Any(x=> x.Hero == null))
				return;
			
			if (AvailableHeroes.Contains (heroViewModel))
				AvailableHeroes.Remove (heroViewModel);
			if (!HeroSlots.Any (x=> x.Hero == heroViewModel.Hero))
				HeroSlots.First(x=> x.Hero == null).SetHero(heroViewModel.Hero);

			RaisePropertyChanged ("StartGame");
		}


		public Command<HeroSlotViewModel> DeselectHeroCommand{
			get{
				return new Command<HeroSlotViewModel> ((heroSlot) => {
					var hero = heroSlot.Hero;

					heroSlot.SetHero(null);

					if (!AvailableHeroes.Any (x=>x.Hero == hero)){
						for(int i= 0; i < AvailableHeroes.Count; i++){
							if(AvailableHeroes[i].Name.CompareTo(hero.Name) == 1){
								AvailableHeroes.Insert (i,new HeroViewModel(hero));
								break;
							}
						}
					}

				},
					(heroSlot)=>{
						return heroSlot.Hero != null;
					});
			}
		}

		public Command StartGame
		{
			get {
				return new Command (
					 async (obj) => {
						Application.CurrentGame.Heroes.Clear();
						Application.CurrentGame.Heroes.AddRange(HeroSlots.Select(x=>x.Hero).ToList());
						Application.CurrentGame.Heroes.ForEach(h=>
							{ 
								h.Secrecy = h.SecrecyDefault;
								h.Grace = h.GraceDefault;
							});
						Application.SaveCurrentGame ();
					await CoreMethods.PushPageModel<BoardSetupPageModel>();
					},
					(obj) => {
						return HeroSlots.Any() && !HeroSlots.Any(x=>x.Hero == null);
					}
				);
			}
		}

		protected override void ViewIsDisappearing(object sender, System.EventArgs e)
		{
			base.ViewIsDisappearing(sender, e);
			//if all heroes where not selected we must be going back to the home screen
			//ensure the the current game is null otherwise you'll be able to 'continue' this game
			if (HeroSlots.Any(x => x.Hero == null))
				Application.CurrentGame = null;
		}
    }

	public class HeroSlotViewModel //:BaseViewModel
	{
		public Hero Hero {get;private set;}
		public ImageSource Image{
			get{
				if (Hero != null)
					return ImageSourceUtil.GetHeroImage (Hero.Name);

				return ImageSource.FromFile ("hero_slot");
			}
		}

		public void SetHero(Hero hero){
			Hero = hero;
			//RaisePropertyChanged (() => Image);
		}
	}

}
