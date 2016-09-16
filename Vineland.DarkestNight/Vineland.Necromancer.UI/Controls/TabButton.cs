using System;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI
{
	public class TabButton : Button
	{
		public TabButton ()
		{
			BackgroundColor = Color.Transparent;
			BorderRadius = 0;
		}

		public static BindableProperty IsSelectedProperty =
			BindableProperty.Create ("IsSelected", typeof(bool), typeof(TabButton), false, BindingMode.TwoWay, propertyChanged: OnIsSelectedChanged);

		public bool IsSelected{
			get{ return (bool)GetValue (IsSelectedProperty); }
			set{ SetValue (IsSelectedProperty, value); }
		}

		private static void OnIsSelectedChanged (BindableObject bindable, object oldvalue, object newvalue)
		{
			var tabButton = bindable as TabButton;

			tabButton.BackgroundColor = tabButton.IsSelected ? AppConstants.HeaderBackground : Color.Transparent;
			tabButton.TextColor = tabButton.IsSelected ? AppConstants.HeaderTextColour : AppConstants.TextColour;
		}
	}
}

