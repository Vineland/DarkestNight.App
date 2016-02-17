using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class HeroPhasePage : CarouselPage
	{
		public HeroPhasePage ()
		{
			InitializeComponent ();
		}

		protected override void OnBindingContextChanged ()
		{
			//Children.Clear ();
			Children.Add (new BlightLocationsPage ());
			foreach (var hero in (BindingContext as HeroPhaseViewModel).Heroes)
				Children.Add (new HeroPage () { BindingContext = hero });
		}
	}
}

