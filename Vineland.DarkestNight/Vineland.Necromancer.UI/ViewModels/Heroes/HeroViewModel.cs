using System;
using Android.Nfc;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Collections.Generic;
using Android.Util;
using XLabs.Ioc;
using Android.OS;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class HeroViewModel : BaseViewModel
	{
		protected Hero _hero;

		public HeroViewModel (Hero hero)
		{
			_hero = hero;
		}

		public string Name {
			get{ return _hero.Name.ToUpper (); }
		}

		public void RemoveHero(){
			var viewModel = new SelectHeroViewModel(Resolver.Resolve<DataService>());
			viewModel.HeroSelected += OnHeroSelected;
			Application.Navigation.Push<SelectHeroPage> (viewModel);
		}

		void OnHeroSelected (object sender, Hero newHero)
		{
			(sender as SelectHeroViewModel).HeroSelected -= OnHeroSelected;

			MessagingCenter.Send<HeroViewModel, HeroDefeatedArgs> (this, "HeroDefeated",
				new HeroDefeatedArgs () {
					DefeatedHero = _hero,
					NewHero = newHero
				});
			
			Application.Navigation.Pop ();
		}


		public void Search(string option){
			var numberOfCards = 0;
			if(int.TryParse(option, out numberOfCards))
				Application.Navigation.Push<SearchPage>(new SearchViewModel(numberOfCards));
		}

		public int Secrecy {
			get{ 
				return _hero.Secrecy; 
			}
			set{ 
				if (_hero.Secrecy != value) {
					_hero.Secrecy = value; 
					MessagingCenter.Send<HeroViewModel, Hero> (this, "HeroUpdated", _hero);
				}
			}
		}

		public List<string> SecrecyOptions {
			get {
				
				var options = new List<string> ();
				for (int i = 0; i <= 7; i++) {
					if (i == _hero.SecrecyDefault)
						options.Add (i + " (Default)");
					else
						options.Add (i.ToString());
				}

				return options;
			}
		}

		public int Grace {
			get{ return _hero.Grace; }
			set{ 
				if (_hero.Grace != value) {
					_hero.Grace = value; 
					MessagingCenter.Send<HeroViewModel, Hero> (this, "HeroUpdated", _hero);
				}
			}
		}


		public List<string> GraceOptions {
			get {

				var options = new List<string> ();
				for (int i = 0; i <= 9; i++) {
					if (i == _hero.GraceDefault)
						options.Add (i + " (Default)");
					else
						options.Add (i.ToString());
				}
				return options;
			}
		}

		public Location Location {
			get{ return Locations.Single (l => l.Id == _hero.LocationId); }
			set {
				if (_hero.LocationId != value.Id) {
					_hero.LocationId = value.Id;
					MessagingCenter.Send<HeroViewModel, Hero> (this, "HeroUpdated", _hero);
				}
			}
		}

		public bool HasVoidArmor {
			get{ return _hero.HasVoidArmor; }
			set {
				if (value)
					Application.CurrentGame.Heroes.ForEach (h => h.HasVoidArmor = false);
				
				_hero.HasVoidArmor = value; 
			}
		}

		public bool HasShieldOfRadiance {
			get { 
				return _hero.HasShieldOfRadiance; 
			}
			set { 
				if (value)
					Application.CurrentGame.Heroes.ForEach (h => h.HasShieldOfRadiance = false);
				
				_hero.HasShieldOfRadiance = value; 
			}
		}

		public List<Location> Locations {
			get { return Application.CurrentGame.Locations; }
		}

		public static HeroViewModel Create (Hero hero)
		{
			if (hero is Seer)
				return new SeerViewModel (hero as Seer);
			else if (hero is Acolyte)
				return new AcolyteViewModel (hero as Acolyte);
			else if (hero is Paragon)
				return new ParagonViewModel (hero as Paragon);
			else if (hero is Ranger)
				return new RangerViewModel (hero as Ranger);
			else if (hero is Scholar)
				return new ScholarViewModel (hero as Scholar);
			else if (hero is Shaman)
				return new ShamanViewModel (hero as Shaman);
			else if (hero is Valkyrie)
				return new ValkyrieViewModel (hero as Valkyrie);
			else if (hero is Wayfarer)
				return new WayfarerViewModel (hero as Wayfarer);
			else if (hero is Wizard)
				return new WizardViewModel (hero as Wizard);
			else if (hero is Conjurer)
				return new ConjurerViewModel (hero as Conjurer);
			else
				return new HeroViewModel (hero);
		}
	}

	public class HeroDefeatedArgs{
		public Hero DefeatedHero;
		public Hero NewHero;
	}
}

