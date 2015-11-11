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
using Vineland.DarkestNight.Core;
using Vineland.DarkestNight.UI.Shared;

namespace Vineland.DarkestNight.UI.Android
{
    [Activity(Label = "New Game")]
    public class NewGameActivity : Activity
    {
        public NewGameViewModel ViewModel { get; set; }

		public NewGameActivity ()
		{
			IoC.BuildUp(this);
		}

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ViewModel.Initialise();

			SetContentView (Resource.Layout.NewGame);

			//set defaults
			FindViewById<EditText>(Resource.Id.StartingDarknessEditText).Text = ViewModel.DarknessLevel.ToString();
			FindViewById<CheckBox>(Resource.Id.PallOfSufferingCheckBox).Checked = ViewModel.PallOfSuffering;

			var modeSpinner = FindViewById<Spinner>(Resource.Id.ModeSpinner);
			modeSpinner.Adapter = new ArrayAdapter<DarknessCardsMode>(this, global::Android.Resource.Layout.SimpleListItem1, ViewModel.DarknessCardsModeOptions);
			modeSpinner.SetSelection((int)ViewModel.Mode);

			FindViewById<Button>(Resource.Id.ChooseHeroesButton).Click += ChooseHeroesButton_Click;

        }

        void ChooseHeroesButton_Click (object sender, EventArgs e)
        {
			//StartActivity(typeof(ChooseHeroesActvity));
        }
    }
}