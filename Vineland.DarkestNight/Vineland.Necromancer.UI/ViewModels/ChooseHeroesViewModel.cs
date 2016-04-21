using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Vineland.DarkestNight.UI;
using Vineland.Necromancer.Core;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class ChooseHeroesViewModel : BaseViewModel
	{
		DataService _dataService;

		public ChooseHeroesViewModel(DataService dataService)
		{
			_dataService = dataService;

			HeroSlots = new ObservableCollection<HeroSlotViewModel> ();
			AvailableHeroes = new ObservableCollection<Hero> ();
			IsLoading = true;
		}

		public void Initialise(){
			
			for (int i = 0; i < Application.CurrentGame.NumberOfPlayers; i++) {
				HeroSlots.Add (new HeroSlotViewModel ());
			}
			foreach (var hero in _dataService.GetAllHeroes())
				AvailableHeroes.Add (hero);
			IsLoading = false;
		}

		public ObservableCollection<HeroSlotViewModel> HeroSlots { get; set; }

		public ObservableCollection<Hero> AvailableHeroes { get; set; }

		public void SelectHero(Hero hero){
			if (!HeroSlots.Any(x=> x.Hero == null))
				return;
			
			if (AvailableHeroes.Contains (hero))
				AvailableHeroes.Remove (hero);
			if (!HeroSlots.Any (x=> x.Hero == hero))
				HeroSlots.First(x=> x.Hero == null).SetHero(hero);

			RaisePropertyChanged (() => StartGame);
		}


		public RelayCommand<HeroSlotViewModel> DeselectHeroCommand{
			get{
				return new RelayCommand<HeroSlotViewModel> ((heroSlot) => {
					var hero = heroSlot.Hero;

					heroSlot.SetHero(null);

					if (!AvailableHeroes.Contains (hero)){
						for(int i= 0; i < AvailableHeroes.Count; i++){
							if(AvailableHeroes[i].Name.CompareTo(hero.Name) == 1){
								AvailableHeroes.Insert (i,hero);
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

		public RelayCommand StartGame
		{
			get {
				return new RelayCommand (
					async () => {
						
						Application.CurrentGame.Heroes = HeroSlots.Select(x=>x.Hero).ToList();
						Application.CurrentGame.Heroes.ForEach(h=>
							{ 
								h.Secrecy = h.SecrecyDefault;
								h.Grace = h.GraceDefault;
							});
						Application.SaveCurrentGame ();
						await Application.Navigation.Push<HeroTurnPage>(clearBackStack: true);
					},
					() => {
						return !HeroSlots.Any(x=>x.Hero == null);
					}
				);
			}
		}

		public override void OnDisappearing ()
		{
			//if all heroes where not selected we must be going back to the home screen
			//ensure the the current game is null otherwise you'll be able to 'continue' this game
			if (HeroSlots.Any(x=>x== null))
				Application.CurrentGame = null;
		}
    }

	public class HeroSlotViewModel:BaseViewModel
	{
		public Hero Hero {get;private set;}
		public ImageSource Image{
			get{
				if (Hero != null)
					return ImageSourceUtil.GetHeroImage (Hero.Name);

				return FileImageSource.FromFile ("hero_slot");
			}
		}

		public void SetHero(Hero hero){
			Hero = hero;
			RaisePropertyChanged (() => Image);
		}
	}
}
