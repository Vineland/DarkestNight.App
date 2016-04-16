using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class CheckButton : Button
	{
		public static BindableProperty IsSelectedProperty = 
			BindableProperty.Create("IsSelected", typeof(bool), typeof(CheckButton), false, BindingMode.TwoWay);
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		public CheckButton ()
		{
			Device.OnPlatform (iOS: () => {
				HorizontalOptions = LayoutOptions.FillAndExpand;
			});
		}
	}
}

