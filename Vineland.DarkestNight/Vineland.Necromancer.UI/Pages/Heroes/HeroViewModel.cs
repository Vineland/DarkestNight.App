using Vineland.Necromancer.Core;
using System.Linq;
using Xamarin.Forms;
using Vineland.Necromancer.Domain;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;

namespace Vineland.Necromancer.UI
{
	public class HeroViewModel : BaseViewModel
	{
		protected Hero _hero;

		public HeroViewModel (Hero hero)
		{
			_hero = hero;
			MessagingCenter.Subscribe<HeroViewModel, Hero>(this, "HeroUpdated", HeroUpdated);
		}

		void HeroUpdated(UI.HeroViewModel sender, Hero updatedHero)
		{
			if (_hero == updatedHero)
			{
				//RaisePropertyChanged(() => HasVoidArmour);
				//RaisePropertyChanged(() => HasShieldOfRadiance);
			}
		}

		public Hero Hero
		{
			get { return _hero; }
		}

		public string Name {
			get{ return _hero.Name; }
		}


		public Command DefeatedCommand {
			get {
				return new Command (async() => {
					HeroDefeated();
				});
			}
		}

		private async void HeroDefeated ()
		{
			if (await CoreMethods.DisplayAlert ("Hero Defeated?", null, "Yes", "No")) {
				//var availableHeroes = Resolver.Resolve<DataService> ().GetAllHeroes ().Where (x => !Application.CurrentGame.Heroes.Any (y => y.Id == x.Id)).ToList ();
				//var newHeroName = await Application.Navigation.DisplayActionSheet ("New Hero", "Cancel", null, availableHeroes.Select (x => x.Name).ToArray ());
				//MessagingCenter.Send<HeroViewModel, HeroDefeatedArgs> (this, "HeroDefeated",
				//	new HeroDefeatedArgs () {
				//		DefeatedHero = _hero,
				//		NewHero = availableHeroes.Single (x => x.Name == newHeroName)
				//	});

				//Application.Navigation.Pop ();
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

		public bool HasVoidArmour
		{
			get { return _hero.HasVoidArmour; }
			set
			{
				if (_hero.HasVoidArmour != value)
				{
					if (value) //if it's set to true then we need to deactivate for anyone else
					{
						var hero = Application.CurrentGame.Heroes.SingleOrDefault(x => x.HasVoidArmour);
						if (hero != null)
						{
							hero.HasVoidArmour = false;
							MessagingCenter.Send<HeroViewModel, Hero>(this, "HeroUpdated", hero);
						}
					}

					_hero.HasVoidArmour = value;
					//MessagingCenter.Send<HeroViewModel, Hero>(this, "HeroUpdated", _hero);
				}
			}
		}

		public bool HasShieldOfRadiance
		{
			get { return _hero.HasShieldOfRadiance; }
			set
			{
				if (_hero.HasShieldOfRadiance != value)
				{
					if (value) //if it's set to true then we need to deactivate for anyone else
					{
						var hero = Application.CurrentGame.Heroes.SingleOrDefault(x => x.HasShieldOfRadiance);
						if (hero != null)
						{
							hero.HasShieldOfRadiance = false;
							MessagingCenter.Send<HeroViewModel, Hero>(this, "HeroUpdated", hero);
						}
					}
					_hero.HasShieldOfRadiance = value;
					//MessagingCenter.Send<HeroViewModel, Hero>(this, "HeroUpdated", _hero);
				}
			}
		}

		public string LocationName
		{
			get
			{
				return Application.CurrentGame.Locations.Single(l => l.Id == _hero.LocationId).Name;
			}
		}

		public Command LocationCommand
		{
			get
			{
				return new Command((obj) =>
				{
					var viewModel = new ChooseLocationPopupPageModel();
					viewModel.OnLocationSelected += LocationSelected;
					CoreMethods.PushPopup(pageModel: viewModel);
				});
			}
		}

		public void LocationSelected(Location location)
		{
			if (_hero.LocationId != location.Id)
			{
				_hero.LocationId = location.Id;
				MessagingCenter.Send<HeroViewModel, Hero>(this, "HeroMoved", _hero);
			}
			RaisePropertyChanged("LocationName");
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

