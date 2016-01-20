using System;
using Xamarin.Forms;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public class PlayerPhase : TabbedPage
	{
		public PlayerPhase ()
		{
			BindingContext = Resolver.Resolve<PlayerPhaseViewModel> ();
			var heroPage = new HeroesState ();
			var locationPage = new BlightLocations ();
			Children.Add (heroPage);
			Children.Add (locationPage);

			var next = new ToolbarItem () { Text = "Necromancer" };
			next.SetBinding<PlayerPhaseViewModel> (ToolbarItem.CommandProperty, vm => vm.NextPhase);
			this.ToolbarItems.Add (next);
		}
	}
}

