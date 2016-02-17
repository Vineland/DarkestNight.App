using System;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using Vineland.DarkestNight.UI;
using System.Runtime.InteropServices;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using Android.Util;
using Xamarin.Forms;
using System.Linq;
using System.Security.Cryptography;

namespace Vineland.Necromancer.UI
{
	public class HeroPhaseViewModel : BaseViewModel
	{
		NavigationService _navigationService;
		GameStateService _gameStateService;

		public HeroPhaseViewModel (NavigationService navigationService, GameStateService gameStateService)
		{
			_gameStateService = gameStateService;
			_navigationService = navigationService;

			MessagingCenter.Subscribe<SelectHeroViewModel,Hero>(this, "HeroSelected", (sender, hero) => {
				//TODO review this at some point
				var fallenHero = Heroes.FirstOrDefault(x=>x.HasFallen);
				var insertIndex = 0;
				if(fallenHero != null)
				{
					insertIndex = Heroes.IndexOf(fallenHero);
					Heroes.Remove(fallenHero);
				}
				Heroes.Insert(insertIndex, hero);
				Heroes = new List<Hero>(Heroes);
				RaisePropertyChanged (() => Heroes);
				_gameStateService.Save();
			});
		}

		public override void Cleanup ()
		{
			MessagingCenter.Unsubscribe<SelectHeroViewModel, Hero> (this, "HeroSelected");
		}

		public int Darkness {
			get { return _gameStateService.CurrentGame.Darkness; }
			set { _gameStateService.CurrentGame.Darkness = value; }
		}

		public List<Hero> Heroes{
			get { return _gameStateService.CurrentGame.Heroes.Active; }
			set{ _gameStateService.CurrentGame.Heroes.Active = value; }
		}

		public Vineland.Necromancer.Core.HeroesState HeroesState{
			get { return _gameStateService.CurrentGame.Heroes; }
		}

		public RelayCommand<Hero> RemoveHero
		{
			get 
			{ 
				return new RelayCommand<Hero> (
					(hero) => { 
						hero.HasFallen = true;
						_navigationService.Push<SelectHeroPage>();
					});
			}
		}

		public List<Location> Locations{
			get { return _gameStateService.CurrentGame.Locations; }
		}

		public RelayCommand NextPhase{
			get{
				return new RelayCommand (() => {
					_gameStateService.Save();
					_navigationService.Push<NecromancerPhasePage>();
				});
			}
		}
	}
}

