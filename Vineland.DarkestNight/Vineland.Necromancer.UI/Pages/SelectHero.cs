using System;
using Xamarin.Forms;
using Dalvik.SystemInterop;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class SelectHero : ContentPage
	{
		public SelectHero ()
		{
			var listView = new ListView (ListViewCachingStrategy.RecycleElement);
			//listView.ItemsSource = ViewModel.AvailableHeroes;
			var cell = new DataTemplate (typeof(TextCell));
			cell.SetBinding(TextCell.TextProperty, "Name");

			listView.ItemTemplate = cell;
			listView.ItemSelected += (sender, e) => {
				//ViewModel.HeroSelected (e.SelectedItem as Hero);
			};

			Content = listView;
		}
	}
}

