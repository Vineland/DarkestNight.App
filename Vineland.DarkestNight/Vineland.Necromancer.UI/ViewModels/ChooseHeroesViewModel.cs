﻿using System;
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
		public ChooseHeroesViewModel(HeroService heroService)
		{
			SelectedHeroes = new ObservableCollection<Hero> ();
			SelectedHeroes.CollectionChanged += (sender, e) => { RaisePropertyChanged(()=> StartGame);};
			Heroes = heroService.GetAll();
		}
 
		public List<Hero> Heroes { get; private set; }

		public ObservableCollection<Hero> SelectedHeroes { get; set; }

		public RelayCommand StartGame{
			get {
				return new RelayCommand (
					() => {
						Application.CurrentGame.Heroes.Active = SelectedHeroes.OrderBy(x=>x.Name).ToList();
						Application.SaveCurrentGame ();
						Application.Navigation.Push<HeroPhasePage>(true);
					},
					() => {
						return SelectedHeroes.Count() == 4;
					}
				);
			}
		}
    }
}
