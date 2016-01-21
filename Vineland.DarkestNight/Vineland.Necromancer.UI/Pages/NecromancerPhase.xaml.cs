using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public partial class NecromancerPhase : ContentPageBase<NecromancerPhaseViewModel>
	{
		public NecromancerPhase ()
		{
			InitializeComponent ();

			//.ItemsSource = Location.All;
		}
	}
}

