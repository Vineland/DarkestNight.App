using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Vineland.DarkestNight.UI.Droid.Infrastructure;
using Vineland.DarkestNight.Core.Services;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI.Droid
{
	[Activity (Theme = "@style/Theme.App", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, Icon = "@drawable/ic_launcher")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			#if RELEASE
			Insights.Initialize("a7b011498c4a5e09ac640c9c044374a16aa995ae", ApplicationContext);
			#endif

			try
			{

				var bootstrapper = new AndroidBootstrapper();
				bootstrapper.Run();

				global::Xamarin.Forms.Forms.Init (this, bundle);

				LoadApplication (new App ());

				AndroidEnvironment.UnhandledExceptionRaiser += new EventHandler<RaiseThrowableEventArgs>(AndroidEnvironment_UnhandledExceptionRaiser);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
		{
			//LogHelper.Log(TAG, "Unhandled Exception: {0}" + e.Exception);
		}
	}
}

