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
	public class HeroesStateViewModel :BaseViewModel
	{
		NavigationService _navigationService;
		SaveGameService _saveGameService;

		public HeroesStateViewModel (NavigationService navigationService, SaveGameService saveGameService)
		{
			_navigationService = navigationService;
			_saveGameService = saveGameService;
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

		public RelayCommand<Hero> RemoveHero
		{
			get 
			{ 
				return new RelayCommand<Hero> (
					(hero) => { 
						hero.HasFallen = true;
						_navigationService.PushViewModel<SelectHeroViewModel>();
					});
			}
		}

		public RelayCommand NextPhase{
			get{
				return new RelayCommand (() => {
					_saveGameService.Save(App.CurrentGame);
					_navigationService.PushViewModel<BlightLocationsViewModel>();
				});
			}
		}
	}
}

