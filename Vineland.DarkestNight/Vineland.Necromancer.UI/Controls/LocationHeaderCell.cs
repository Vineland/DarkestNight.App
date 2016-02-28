using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Vineland.Necromancer.UI
{
	public class LocationHeaderCell:ViewCell
	{
		public LocationHeaderCell ()
		{
		}

		protected override void OnBindingContextChanged ()
		{
			//var huh = BindingContext as ObservableCollection;
			var label = new Label();
			label.SetBinding (Label.TextProperty, new Binding ("Key"));
			View = label;
		}
	}
}

