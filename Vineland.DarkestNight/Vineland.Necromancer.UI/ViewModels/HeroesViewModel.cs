﻿using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Vineland.Necromancer.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class HeroesViewModel :BaseViewModel
	{
		public HeroesViewModel ()
		{
			Heroes = new ObservableCollection<HeroSummaryViewModel> ();

			MessagingCenter.Subscribe<HeroViewModel, HeroDefeatedArgs> (this, "HeroDefeated", OnHeroDefeated);
			Initialise ();
		}

		public void Initialise(){
			foreach (var hero in Application.CurrentGame.Heroes.Active)
				Heroes.Add (new HeroSummaryViewModel (hero));
			
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<HeroViewModel,Hero> (this, "HeroDefeated");
		}

		public ObservableCollection<HeroSummaryViewModel> Heroes { get; private set; }

		public bool AcolytePresent { get { return Application.CurrentGame.Heroes.Active.GetHero<Acolyte> () != null; } }
		public bool ConjurerPresent { get { return Application.CurrentGame.Heroes.Active.GetHero<Conjurer> () != null; } }
		public bool ParagonPresent { get { return Application.CurrentGame.Heroes.Active.GetHero<Paragon> () != null; } }
		public bool RangerPresent { get { return Application.CurrentGame.Heroes.Active.GetHero<Ranger> () != null; } }
		public bool ScholarPresent { get { return Application.CurrentGame.Heroes.Active.GetHero<Scholar> () != null; } }
		public bool SeerPresent { get { return Application.CurrentGame.Heroes.Active.GetHero<Seer> () != null; } }
		public bool ValkyriePresent { get { return Application.CurrentGame.Heroes.Active.GetHero<Valkyrie> () != null; } }
		public bool WizardPresent { get { return Application.CurrentGame.Heroes.Active.GetHero<Wizard> () != null; } }
		public bool WayfarerPresent { get { return Application.CurrentGame.Heroes.Active.GetHero<Wayfarer> () != null; } }

		public HeroesState HeroesState { get { return Application.CurrentGame.Heroes; } }

		private void OnHeroDefeated (HeroViewModel sender, HeroDefeatedArgs args)
		{
			var model = Heroes.FirstOrDefault (x => x.Hero.Id == args.DefeatedHero.Id);
			if (model != null)
				Heroes.Remove (model);

			args.NewHero.Grace = args.NewHero.GraceDefault;
			args.NewHero.Secrecy = args.NewHero.SecrecyDefault;

			Heroes.Add(new HeroSummaryViewModel (args.NewHero));

			Task.Run (() => {
				Application.CurrentGame.Heroes.Active.Remove (args.DefeatedHero);
				Application.CurrentGame.Heroes.Active.Add (args.NewHero);
				Application.SaveCurrentGame ();
			});
		}
	}

	public class HeroSummaryViewModel: BaseViewModel
	{
		public Hero Hero;

		public HeroSummaryViewModel (Hero hero)
		{
			Hero = hero;
		}

		public ImageSource Image 
		{
			get { return ImageSource.FromFile (Hero.Name.Replace (" ", string.Empty).ToLower ()); }
		}

		public string Name { get { return Hero.Name.ToUpper (); } }

		public Location Location 
		{
			get{ return Locations.Single (l => l.Id == Hero.LocationId); }
			set {
				if (Hero.LocationId != value.Id) {
					Hero.LocationId = value.Id;
				}
			}
		}

		public List<Location> Locations {
			get { return Application.CurrentGame.Locations; }
		}

		public int Secrecy { get { return Hero.Secrecy; } 
			set{ Hero.Secrecy = value; }}

		public int Grace { get { return Hero.Grace; } 
			set { Hero.Grace = value; }}
		
		public List<string> SecrecyOptions {
			get {

				var options = new List<string> ();
				for (int i = 0; i <= 7; i++) {
					if (i == Hero.SecrecyDefault)
						options.Add (i + " (Default)");
					else
						options.Add (i.ToString ());
				}

				return options;
			}
		}


		public List<string> GraceOptions {
			get {

				var options = new List<string> ();
				for (int i = 0; i <= 9; i++) {
					if (i == Hero.GraceDefault)
						options.Add (i + " (Default)");
					else
						options.Add (i.ToString ());
				}
				return options;
			}
		}
	}

}

