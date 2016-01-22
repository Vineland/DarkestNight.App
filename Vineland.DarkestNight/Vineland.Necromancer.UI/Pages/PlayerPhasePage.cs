using System;
using Xamarin.Forms;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public class PlayerPhasePage : TabbedPage
	{
		public PlayerPhasePage ()
		{
			var pageCreator = Resolver.Resolve<PageService>();
			Children.Add (pageCreator.CreatePage<HeroesStatePage>());
			Children.Add (pageCreator.CreatePage<BlightLocationsPage>());

			var next = new ToolbarItem () { Text = "Necromancer" };
			next.SetBinding<PlayerPhaseViewModel> (ToolbarItem.CommandProperty, vm => vm.NextPhase);
			this.ToolbarItems.Add (next);
		}
	}
}

