using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vineland.Necromancer.Core;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Android.Util;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI
{
	public class ChooseHeroesViewModel : BaseViewModel
    {
		NavigationService _navigationService;
		GameStateService _gameStateService;

		public ChooseHeroesViewModel(GameStateService gameStateService, NavigationService navigationService)
		{
			_gameStateService = gameStateService;
			_navigationService = navigationService;
			SelectedHeroes = new ObservableCollection<Hero> ();
			SelectedHeroes.CollectionChanged += (sender, e) => { RaisePropertyChanged(()=> StartGame);};
			Heroes = Hero.All;
		}
 
		public List<Hero> Heroes { get; private set; }

		public ObservableCollection<Hero> SelectedHeroes { get; set; }

		public RelayCommand StartGame{
			get {
				return new RelayCommand (
					() => {
						_gameStateService.CurrentGame.Heroes.Active = SelectedHeroes.ToList();
						_gameStateService.Save ();
						_navigationService.Push<HeroPhasePage>(true);
					},
					() => {
						return SelectedHeroes.Count() == 4;
					}
				);
			}
		}
    }
}
