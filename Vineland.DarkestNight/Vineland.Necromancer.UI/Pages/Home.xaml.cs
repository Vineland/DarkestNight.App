using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using GalaSoft.MvvmLight;
using Android.Views.InputMethods;

namespace Vineland.Necromancer.UI
{
	public partial class Home : ContentPageBase<HomeViewModel>
	{
		public Home ()
		{
			InitializeComponent ();
			Title = "Home";
		}
	}
}

