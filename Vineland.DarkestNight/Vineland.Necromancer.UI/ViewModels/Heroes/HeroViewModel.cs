using System;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Collections.Generic;
using XLabs.Ioc;
using Xamarin.Forms;
using Vineland.Necromancer.Domain;

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
			get{ return _hero.Name; }
		}


		public RelayCommand DefeatedCommand {
			get {
				return new RelayCommand (async () => {
					HeroDefeated();
				});
			}
		}

		private async void HeroDefeated ()
		{
			if (await Application.Navigation.DisplayConfirmation ("Hero Defeated?", null, "Yes", "No")) {
				var availableHeroes = Resolver.Resolve<DataService> ().GetAllHeroes ().Where (x => !Application.CurrentGame.Heroes.Any (y => y.Id == x.Id)).ToList ();
				var newHeroName = await Application.Navigation.DisplayActionSheet ("New Hero", "Cancel", null, availableHeroes.Select (x => x.Name).ToArray ());
				MessagingCenter.Send<HeroViewModel, HeroDefeatedArgs> (this, "HeroDefeated",
					new HeroDefeatedArgs () {
						DefeatedHero = _hero,
						NewHero = availableHeroes.Single (x => x.Name == newHeroName)
					});

				Application.Navigation.Pop ();
			}
		}

		public ImageSource Image
		{
			get
			{
				return ImageSourceUtil.GetHeroImage(_hero.Name);
			}
		}

		public int Secrecy {
			get { 
				return _hero.Secrecy; 
			}
			set { 
				if (_hero.Secrecy != value) {
					_hero.Secrecy = value; 
					MessagingCenter.Send<HeroViewModel, Hero> (this, "HeroUpdated", _hero);
				}
			}
		}

		public int Grace {
			get{ return _hero.Grace; }
			set { 
				if (_hero.Grace != value) {
					_hero.Grace = value; 
					MessagingCenter.Send<HeroViewModel, Hero> (this, "HeroUpdated", _hero);
				}
			}
		}

		public Location Location {
			get{ return Locations.Single (l => l.Id == _hero.LocationId); }
			set {
				if (_hero.LocationId != value.Id) {
					_hero.LocationId = value.Id;
					MessagingCenter.Send<HeroViewModel, Hero> (this, "HeroMoved", _hero);
				}
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
			else if (hero is Tamer)
				return new TamerViewModel (hero as Tamer);
			else
				return new HeroViewModel (hero);
		}
	}

	public class HeroDefeatedArgs
	{
		public Hero DefeatedHero;
		public Hero NewHero;
	}
}

