using Android.App;
using Android.Runtime;
using System;
using Vineland.DarkestNight.UI.Android.Infrastructure;
using Vineland.DarkestNight.UI.Shared;
using Vineland.DarkestNight.Core.Services;

namespace Vineland.DarkestNight.UI.Android
{
	[Application(Label = "Necromancer", Theme = "@style/Theme.Splash", Icon = "@drawable/ic_launcher")]
    public class DarkestNightApplication : Application
    {
		readonly string TAG = string.Format("{0} {1}", AppConstants.AppName,  "DarkestNightApplication");

        public DarkestNightApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

		public override void OnCreate()
		{
			#if RELEASE
			Insights.Initialize("a7b011498c4a5e09ac640c9c044374a16aa995ae", ApplicationContext);
			#endif

			try
			{
				base.OnCreate();

				var bootstrapper = new AndroidBootstrapper();
				bootstrapper.Run();

				//create the services
				IoC.BuildUp(this);

				AndroidEnvironment.UnhandledExceptionRaiser += new EventHandler<RaiseThrowableEventArgs>(AndroidEnvironment_UnhandledExceptionRaiser);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
		{
			LogHelper.Log(TAG, "Unhandled Exception: {0}" + e.Exception);
		}

    }
}
