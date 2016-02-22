using System;
using Xamarin.Forms;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;

namespace Vineland.Necromancer.UI
{
	public class HeroViewModel : BaseViewModel
	{
		NavigationService _navigationService;
		GameStateService _gameStateService;

		public HeroViewModel (Hero hero, NavigationService navigationService, GameStateService gameStateService)
		{
			_gameStateService = gameStateService;
			_navigationService = navigationService;
			Hero = hero;
		}

		public Hero Hero { get; set; }

	}
}

