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

namespace Vineland.DarkestNight.UI.Android
{
    public class BaseActivity:Activity
    {
        public BaseActivity()
        {
            //TODO: abstract out the actual IoC implementation
            TinyIoC.TinyIoCContainer.Current.BuildUp(this);
        }
    }
}