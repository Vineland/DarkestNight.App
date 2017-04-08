using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class BlightsPage : ContentPage
	{
		public BlightsPage ()
		{
			InitializeComponent ();

LocationsListView.ItemSelected += (sender, e) => {
    ((ListView)sender).SelectedItem = null;
};
		}
	}
}

