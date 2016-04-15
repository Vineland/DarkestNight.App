using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using GalaSoft.MvvmLight;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing ()
		{
			(BindingContext as BaseViewModel).OnAppearing ();
		}
	}
}

