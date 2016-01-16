using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Dynamic;

namespace Vineland.Necromancer.UI
{
	public partial class HeroesPhase : ContentPageBase<HeroesPhaseViewModel>
	{
		public HeroesPhase ()
		{
			InitializeComponent ();

		}

		protected override bool OnBackButtonPressed ()
		{
			//return false;

			NavigationService.PopToRoot ();

			return true;
		}
	}
}

