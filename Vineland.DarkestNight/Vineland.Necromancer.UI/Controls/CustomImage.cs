using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class CustomImage: Image 
	{
		public CustomImage()
		{
		}



		public static BindableProperty OnLongPressProperty =
			BindableProperty.Create("OnLongPress", typeof(ICommand), typeof(ItemsSourceStackLayout), null);


		public ICommand OnLongPress
		{
			get
			{
				return (ICommand)GetValue(OnLongPressProperty);
			}
			set
			{
				SetValue(OnLongPressProperty, value);
			}
		}

		public static BindableProperty OnTapProperty =
			BindableProperty.Create("OnTap", typeof(ICommand), typeof(ItemsSourceStackLayout), null);


		public ICommand OnTap
		{
			get
			{
				return (ICommand)GetValue(OnTapProperty);
			}
			set
			{
				SetValue(OnTapProperty, value);
			}
		}
	}
}
