using System;
using System.Collections.Generic;
using Vineland.DarkestNight.Core;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class SelectHeroViewModel :BaseViewModel
	{
		NavigationService _navigationService;
		SaveGameService _saveGameService;

		public SelectHeroViewModel (NavigationService navigationService, SaveGameService saveGameService)
		{
			_navigationService = navigationService;
			_saveGameService = saveGameService;
		}

		public IList<Hero> AvailableHeroes
		{
			get { return Hero.All.Where (x => !App.CurrentGame.Heroes.Active.Any(y => y.Id == x.Id)).ToList(); }
		}

		public void HeroSelected(Hero hero)
		{
			MessagingCenter.Send<SelectHeroViewModel, Hero>(this, "HeroSelected", hero);
			_navigationService.Pop ();
		}
	}
}

