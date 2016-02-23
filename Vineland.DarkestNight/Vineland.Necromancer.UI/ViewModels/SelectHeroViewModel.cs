using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class SelectHeroViewModel :BaseViewModel
	{
		NavigationService _navigationService;
		GameStateService _gameStateService;

		public SelectHeroViewModel (NavigationService navigationService, GameStateService gameStateService, HeroService heroService)
		{
			_navigationService = navigationService;
			_gameStateService = gameStateService;
			AvailableHeroes = heroService.GetAll().Where(x => !_gameStateService.CurrentGame.Heroes.Active.Any(y => y.Id == x.Id)).ToList();
		}

		public IList<Hero> AvailableHeroes { get; private set; }

		Hero _selectedHero;
		public Hero SelectedHero
		{ 
			get { return _selectedHero; } 
			set {
				_selectedHero = value;
				if (value != null) {
					MessagingCenter.Send<SelectHeroViewModel, Hero> (this, "HeroSelected", value);
					_navigationService.Pop ();
				}
			}
		}
	}
}

