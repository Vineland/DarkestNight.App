﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Vineland.Necromancer.UI.Droid
{
	[Activity(Theme = "@style/Theme.Splash", MainLauncher = true,Label = "Necro", NoHistory = true, Icon = "@drawable/ic_launcher")]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			StartActivity(typeof(MainActivity));
		}
	}
}

