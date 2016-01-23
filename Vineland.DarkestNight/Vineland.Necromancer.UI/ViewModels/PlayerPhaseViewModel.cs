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
	public class PlayerPhaseViewModel : BaseViewModel
	{
		NavigationService _navigationService;
		SaveGameService _saveGameService;

		public PlayerPhaseViewModel (NavigationService navigationService, SaveGameService saveGameService)
		{
			_saveGameService = saveGameService;
			_navigationService = navigationService;

			MessagingCenter.Subscribe<SelectHeroViewModel,Hero>(this, "HeroSelected", (sender, hero) => {
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
			});
		}

		public override void Cleanup ()
		{
			MessagingCenter.Unsubscribe<SelectHeroViewModel, Hero> (this, "HeroSelected");
		}

		public int DarknessLevel {
			get { return App.CurrentGame.DarknessLevel; }
			set { App.CurrentGame.DarknessLevel = value; }
		}

		public List<Hero> Heroes{
			get { return App.CurrentGame.Heroes.Active; }
			set{ App.CurrentGame.Heroes.Active = value; }
		}

		public Vineland.Necromancer.Core.HeroesState HeroesState{
			get { return App.CurrentGame.Heroes; }
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
			get { return App.CurrentGame.Locations; }
		}

		public RelayCommand NextPhase{
			get{
				return new RelayCommand (() => {
					_saveGameService.Save(App.CurrentGame);
					_navigationService.Push<NecromancerPhasePage>();
				});
			}
		}
	}
}

