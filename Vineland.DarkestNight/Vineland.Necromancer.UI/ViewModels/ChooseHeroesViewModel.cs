using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Android.Util;
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
			Heroes = dataService.GetAllHeroes();
		}
 
		public List<Hero> Heroes { get; private set; }

		public ObservableCollection<Hero> SelectedHeroes { get; set; }

		public RelayCommand StartGame
		{
			get {
				return new RelayCommand (
					() => {
						Application.CurrentGame.Heroes = SelectedHeroes.ToList();
						Application.CurrentGame.Heroes.ForEach(h=>
							{ 
								h.Secrecy = h.SecrecyDefault;
								h.Grace = h.GraceDefault;
							});
						Application.SaveCurrentGame ();
						Application.Navigation.Push<ActiveHeroesPage>(clearBackStack: true);
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
