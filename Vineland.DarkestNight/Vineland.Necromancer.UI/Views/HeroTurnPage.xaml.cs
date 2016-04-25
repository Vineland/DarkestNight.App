using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.Necromancer.Core;
using System.Threading.Tasks;

namespace Vineland.Necromancer.UI
{
	public partial class HeroTurnPage : ContentPage
	{
		public HeroTurnPage ()
		{ 
			InitializeComponent ();
			ListView.ItemTapped += ListView_ItemTapped;
		}

		void ListView_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			ListView.SelectedItem = null;

			var hero = (e.Item as HeroSummaryViewModel)?.Hero;
			if (hero == null)
				return;
			
			(BindingContext as HeroTurnViewModel).HeroSelected (hero); 	
		}
	}
}

