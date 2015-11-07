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
using Vineland.DarkestNight.UI.Shared.ViewModels;

namespace Vineland.DarkestNight.UI.Android
{
    [Activity(Label = "New Game")]
    public class NewGameActivity : BaseActivity
    {
        public NewGameViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ViewModel.Initialise();
        }
    }
}