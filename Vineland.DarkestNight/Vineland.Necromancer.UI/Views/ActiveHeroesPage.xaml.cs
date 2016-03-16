using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class ActiveHeroesPage : ContentPage
	{
		public ActiveHeroesPage ()
		{
			InitializeComponent ();
			ListView.ItemTapped += ListView_ItemTapped;
		}

		void ListView_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			if (e == null)
				return; // has been set to null, do not 'process' tapped event
			
			((ListView)sender).SelectedItem = null; // de-select the row
		}
	}
}

