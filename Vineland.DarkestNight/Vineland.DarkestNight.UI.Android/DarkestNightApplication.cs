using Android.App;
using Android.Runtime;
using System;
using Vineland.DarkestNight.UI.Android.Infrastructure;

namespace Vineland.DarkestNight.UI.Android
{
    [Application]
    public class DarkestNightApplication : Application
    {
        public DarkestNightApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            var bootstrapper = new AndroidBootstrapper();
            bootstrapper.Register();
        }
    }
}
}