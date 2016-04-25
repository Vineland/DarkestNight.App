using System;
using Vineland.Necromancer.Core;
using Xamarin.Forms;
using System.Linq;

namespace Vineland.Necromancer.UI
{
	public class HeroSummaryViewModel: BaseViewModel
	{
		public Hero Hero;

		public HeroSummaryViewModel (Hero hero)
		{
			Hero = hero;
		}

		public ImageSource Image {
			get { return ImageSource.FromFile (Hero.Name.Replace (" ", string.Empty).ToLower ()); }
		}

		public string Name { get { return Hero.Name.ToUpper (); } }

		public string Location { 
			get { 
				var location = Application.CurrentGame.Locations.Single (l => l.Id == Hero.LocationId);
				return location.Name;
			}
		}

		public int Secrecy { get { return Hero.Secrecy; } }

		public int Grace { get { return Hero.Grace; } }

		public void Updated()
		{
			RaisePropertyChanged (() => Secrecy);
			RaisePropertyChanged (() => Grace);
			RaisePropertyChanged (() => Location);
		}
	}
}

