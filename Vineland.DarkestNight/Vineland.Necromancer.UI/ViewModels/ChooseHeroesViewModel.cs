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

namespace Vineland.Necromancer.UI
{
	public class ChooseHeroesViewModel : BaseViewModel
    {
		public ChooseHeroesViewModel(DataService dataService)
		{
			SelectedHeroes = new ObservableCollection<Hero> ();
			SelectedHeroes.CollectionChanged += (sender, e) => { RaisePropertyChanged(()=> StartGame);};
			AvailableHeroes = new ObservableCollection<Hero>(dataService.GetAllHeroes());
		}
 
		public ObservableCollection<Hero> SelectedHeroes { get; set; }

		public ObservableCollection<Hero> AvailableHeroes { get; set; }

		public void SelectHero(Hero hero){
			if (SelectedHeroes.Count == 4)
				return;
			
			if (AvailableHeroes.Contains (hero))
				AvailableHeroes.Remove (hero);
			if (!SelectedHeroes.Contains (hero))
				SelectedHeroes.Add (hero);
		}


		public RelayCommand<Hero> DeselectHeroCommand{
			get{
				return new RelayCommand<Hero> (async (hero) => {

					if (SelectedHeroes.Contains (hero))
						SelectedHeroes.Remove (hero);
					
					if (!AvailableHeroes.Contains (hero)){
						for(int i= 0; i < AvailableHeroes.Count; i++){
							if(AvailableHeroes[i].Name.CompareTo(hero.Name) == 1){
								AvailableHeroes.Insert (i,hero);
								break;
							}
						}
					}
				});
			}
		}

		public RelayCommand StartGame
		{
			get {
				return new RelayCommand (
					async () => {
						
						Application.CurrentGame.Heroes = SelectedHeroes.ToList();
						Application.CurrentGame.Heroes.ForEach(h=>
							{ 
								h.Secrecy = h.SecrecyDefault;
								h.Grace = h.GraceDefault;
							});
						Application.SaveCurrentGame ();
						await Application.Navigation.Push<HeroTurnPage>(clearBackStack: true);
					},
					() => {
						return SelectedHeroes.Count() == 4;
					}
				);
			}
		}

		public override void OnDisappearing ()
		{
			//if all heroes where not selected we must be going back to the home screen
			//ensure the the current game is null otherwise you'll be able to 'continue' this game
			if (SelectedHeroes.Count != 4)
				Application.CurrentGame = null;
		}
    }
}
