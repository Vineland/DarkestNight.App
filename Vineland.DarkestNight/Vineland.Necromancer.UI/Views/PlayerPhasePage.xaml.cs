using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class PlayerPhasePage : CarouselPage
	{
		public PlayerPhasePage ()
		{
			InitializeComponent ();
		}

		protected override void OnBindingContextChanged ()
		{
			Children.Clear ();
			Children.Add (new BlightLocationsPage ());
			foreach (var hero in (BindingContext as PlayerPhaseViewModel).Heroes)
				Children.Add (new HeroPage () { BindingContext = hero });
		}
	}
}

