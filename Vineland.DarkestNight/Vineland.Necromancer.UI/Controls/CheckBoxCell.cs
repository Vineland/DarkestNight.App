using System;

using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class CheckBoxCell : ViewCell
	{

		public static BindableProperty IsSelectedProperty = BindableProperty.Create<CheckBoxCell, bool>(o => o.IsSelected, false);
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		public static BindableProperty TextProperty = BindableProperty.Create<CheckBoxCell, string>(o => o.Text, string.Empty);
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public CheckBoxCell ()
		{
		}
	}
}


