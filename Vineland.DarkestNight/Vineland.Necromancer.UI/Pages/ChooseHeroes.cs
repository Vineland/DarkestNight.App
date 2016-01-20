using System;
using Vineland.DarkestNight.Core;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class ChooseHeroes : ContentPageBase<ChooseHeroesViewModel>
	{
		public ChooseHeroes ()
		{
			Title = "Choose Heroes";

			var listView = new MultiSelectListView<Hero> () {};
			listView.ItemsSource = ViewModel.Heroes;
			listView.SetBinding<ChooseHeroesViewModel> (MultiSelectListView<Hero>.SelectedItemsProperty, vm => vm.SelectedHeroes);

			var startGame = new ToolbarItem () { Text = "Start Game"};
			startGame.SetBinding<ChooseHeroesViewModel> (ToolbarItem.CommandProperty, vm => vm.StartGame);	
			this.ToolbarItems.Add (startGame);

			Content = listView;
		}
	}
}

