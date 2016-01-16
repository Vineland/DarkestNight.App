using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class CheckButton : Button
	{
		public static BindableProperty IsSelectedProperty = BindableProperty.Create<CheckButton, bool>(o => o.IsSelected, false);
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		public CheckButton ()
		{
		}
	}
}

