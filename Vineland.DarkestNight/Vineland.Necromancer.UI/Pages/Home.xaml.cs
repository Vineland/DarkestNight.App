using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using Vineland.DarkestNight.UI.ViewModels;
using GalaSoft.MvvmLight;
using Android.Views.InputMethods;

namespace Vineland.Necromancer.UI
{
	public partial class Home : ContentPage
	{
		public Home ()
		{
			InitializeComponent ();

			BindingContext = IoC.Get<HomeViewModel> ();
		}
	}
}

