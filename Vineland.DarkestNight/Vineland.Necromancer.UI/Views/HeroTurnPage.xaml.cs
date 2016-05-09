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
			HeroesListView.ItemTapped += ListView_ItemTapped;
			HeroesToggle.Clicked += Toggle_Clicked;
		}

		void Toggle_Clicked (object sender, EventArgs e)
		{
			HeroesListView.IsVisible = !HeroesListView.IsVisible;
			BlightsListView.IsVisible = !HeroesListView.IsVisible;
		}

		void ListView_ItemTapped (object sender, ItemTappedEventArgs e)
		{
			HeroesListView.SelectedItem = null;

			var hero = (e.Item as HeroSummaryViewModel)?.Hero;
			if (hero == null)
				return;
			
			(BindingContext as HeroTurnViewModel).HeroSelected (hero); 	
		}
	}
}

