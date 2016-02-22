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
using System.Threading.Tasks;

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

			Heroes = new ObservableCollection<Hero> (_gameStateService.CurrentGame.Heroes.Active);

			MessagingCenter.Subscribe<SelectHeroViewModel,Hero> (this, "HeroSelected", OnHeroSelected);
		}

		public override void Cleanup ()
		{
			base.Cleanup ();
			MessagingCenter.Unsubscribe<SelectHeroViewModel,Hero> (this, "HeroSelected");
		}

		public ObservableCollection<Hero> Heroes { get; private set; }

		public Hero SelectedHero {get;set;}

		public RelayCommand NextPhase {
			get {
				return new RelayCommand (() => {
					_gameStateService.Save ();
					_navigationService.Push<NecromancerPhasePage> ();
				});
			}
		}


		Hero _heroToRemove;

		public RelayCommand<Hero> RemoveHero {
			get { 
				return new RelayCommand<Hero> (
					(hero) => { 
						_heroToRemove = hero;
						_navigationService.Push<SelectHeroPage> ();
					});
			}
		}

		private void OnHeroSelected (SelectHeroViewModel sender, Hero selectedHero)
		{
			//no hero was selected i.e they backed out or cancelled
			if (selectedHero == null)
				return;

			//remove current hero
			var index = Heroes.IndexOf (_heroToRemove);
			Heroes.Remove (_heroToRemove);
			Heroes.Insert (index, selectedHero);
			SelectedHero = selectedHero;
			RaisePropertyChanged (() => SelectedHero);
			Task.Run (() => {
				_gameStateService.CurrentGame.Heroes.Active = Heroes.ToList ();
				_gameStateService.Save ();
			});
		}


		public RelayCommand Blights {
			get {
				return new RelayCommand (() => {
					_navigationService.Push<BlightLocationsPage> ();
				});
			}
		}

		public int Darkness {
			get { return _gameStateService.CurrentGame.Darkness; }
			set {
				_gameStateService.CurrentGame.Darkness = value;
				RaisePropertyChanged (() => Darkness);
			}
		}


		public Vineland.Necromancer.Core.HeroesState HeroesState {
			get { return _gameStateService.CurrentGame.Heroes; }
		}

	}
}

