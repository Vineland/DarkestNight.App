using System;
using Xamarin.Forms;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public class PlayerPhase : TabbedPage
	{
		public PlayerPhase ()
		{
			Children.Add (new HeroesState ());
			Children.Add (new BlightLocations ());

			var next = new ToolbarItem () { Text = "Necromancer" };
			next.SetBinding<PlayerPhaseViewModel> (ToolbarItem.CommandProperty, vm => vm.NextPhase);
			this.ToolbarItems.Add (next);
		}
	}
}

