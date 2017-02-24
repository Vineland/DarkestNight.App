using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Vineland.DarkestNight.UI.Droid.Infrastructure;
using Vineland.Necromancer.Core.Services;
using Vineland.DarkestNight.UI;
using Xamarin;
using CarouselView.FormsPlugin.Android;

namespace Vineland.Necromancer.UI.Droid
{

	[Activity(Theme = "@style/Theme.Splash", MainLauncher = true,Label = "Necro", Icon = "@drawable/ic_launcher", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.SetTheme(Resource.Style.Theme_AppTheme);
			base.OnCreate (bundle);

			//#if RELEASE
			Insights.Initialize("0b86252c1fff1cce0924366fd12f82f6c55f29aa", ApplicationContext);
			//#endif

			try
			{
				LogHelper.Info("Bootstrapper");
				var bootstrapper = new AndroidBootstrapper();
				bootstrapper.Run();

				LogHelper.Info("Initialising Forms");
				global::Xamarin.Forms.Forms.Init (this, bundle);
				CarouselViewRenderer.Init();

				LogHelper.Info("Loading  NecromancerApp");
				LoadApplication (new NecromancerApp ());

				AndroidEnvironment.UnhandledExceptionRaiser += new EventHandler<RaiseThrowableEventArgs>(AndroidEnvironment_UnhandledExceptionRaiser);
			}
			catch (Exception ex)
			{
				LogHelper.Error(ex);
				throw;
			}
		}

		private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
		{
			LogHelper.Error(e.Exception);
		}
	}
}

