using System;
using Xamarin.Forms;
using Dalvik.SystemInterop;
using Vineland.DarkestNight.Core;

namespace Vineland.Necromancer.UI
{
	public class SelectHero : ContentPageBase<SelectHeroViewModel>
	{
		public SelectHero ()
		{
		}

		protected override void OnBindingContextChanged ()
		{
			var listView = new ListView ();
			listView.SetBinding<SelectHeroViewModel> (ListView.ItemsSourceProperty, vm => vm.AvailableHeroes);
			listView.ItemSelected += (sender, e) => {
				ViewModel.HeroSelected (e.SelectedItem as Hero);
			};

			Content = listView;
		}
	}
}

