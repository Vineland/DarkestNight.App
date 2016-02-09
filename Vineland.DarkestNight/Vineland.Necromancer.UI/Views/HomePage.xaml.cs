using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using GalaSoft.MvvmLight;
using Android.Views.InputMethods;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
			Title = "Home";
			BackgroundImage = "background.jpg";
			NavigationPage.SetHasNavigationBar (this, false);
		}
	}
}

