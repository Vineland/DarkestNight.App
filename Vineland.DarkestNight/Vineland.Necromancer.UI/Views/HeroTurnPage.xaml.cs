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
			HeroesToggle.Clicked += HeroesToggle_Clicked;
			LocationsToggle.Clicked += LocationsToggle_Clicked;
		}

		void HeroesToggle_Clicked (object sender, EventArgs e)
		{
			if (HeroesToggle.IsSelected)
				return;
			
			HeroesToggle.IsSelected = 
			HeroesListView.IsVisible = true;

			LocationsToggle.IsSelected = 
				BlightsListView.IsVisible = false;
		}

		void LocationsToggle_Clicked (object sender, EventArgs e)
		{
			if (LocationsToggle.IsSelected)
				return;

			HeroesToggle.IsSelected = 
				HeroesListView.IsVisible = false;

			LocationsToggle.IsSelected = 
				BlightsListView.IsVisible = true;
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

