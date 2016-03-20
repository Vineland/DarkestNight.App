using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class MapCardPage : ContentPage
	{
		public MapCardPage ()
		{
			InitializeComponent ();
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			if (BindingContext == null)
				return;

			if ((BindingContext as MapCardViewModel).Context != MapCardViewModel.MapCardContext.Search) {
				var doneButton = new ToolbarItem ();
				doneButton.SetBinding<MapCardViewModel> (ToolbarItem.TextProperty, vm => vm.DoneLabel);
				doneButton.SetBinding<MapCardViewModel> (ToolbarItem.CommandProperty, vm => vm.DoneCommand);
				ToolbarItems.Add (doneButton);
			}
		}
	}
}

