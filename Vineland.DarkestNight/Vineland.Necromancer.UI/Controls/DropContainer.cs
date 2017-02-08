using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class DropContainer :StackLayout
	{
		public DropContainer()
		{
		}

		public static BindableProperty OnDropProperty =
			BindableProperty.Create("OnDrop", typeof(RelayCommand<object>), typeof(DropContainer), null);


		public RelayCommand<object> OnDrop
		{
			get
			{
				return (RelayCommand<object>)GetValue(OnDropProperty);
			}
			set
			{
				SetValue(OnDropProperty, value);
			}
		}
	}
}
