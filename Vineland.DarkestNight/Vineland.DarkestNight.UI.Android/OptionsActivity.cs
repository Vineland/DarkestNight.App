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
using Vineland.DarkestNight.UI.Shared.Services;
using Android.Media;
using Android.Views.TextService;

namespace Vineland.DarkestNight.UI.Android
{
    [Activity(Label = "Options")]
    public class OptionsActivity : Activity
    {
        public OptionsViewModel ViewModel { get; set; }
		public ISettingsService Settings {get;set;}

		public OptionsActivity ()
		{
			IoC.BuildUp(this);
		}

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			SetContentView (Resource.Layout.Options);

			var startingDarknessEditText = FindViewById<EditText>(Resource.Id.StartingDarknessEditText);
			startingDarknessEditText.Text = ViewModel.StartingDarkness.ToString();
			startingDarknessEditText.TextChanged += (sender, e) => { ViewModel.StartingDarkness = (sender as EditText).Text;};

			var pallOfSufferingCheckBox = FindViewById<CheckBox>(Resource.Id.PallOfSufferingCheckBox);
			pallOfSufferingCheckBox.Checked = ViewModel.PallOfSuffering;
			pallOfSufferingCheckBox.CheckedChange += (sender, e) => {ViewModel.PallOfSuffering = e.IsChecked;};

			var modeSpinner = FindViewById<Spinner>(Resource.Id.ModeSpinner);
			modeSpinner.Adapter = new ArrayAdapter<DarknessCardsMode>(this, global::Android.Resource.Layout.SimpleListItem1, ViewModel.DarknessCardsModeOptions);
			modeSpinner.SetSelection((int)ViewModel.DarknessCardsMode);
			modeSpinner.ItemSelected += (sender, e) => {ViewModel.DarknessCardsMode = (DarknessCardsMode)e.Position;};

			var alwaysUseDefaultCheckBox = FindViewById<CheckBox>(Resource.Id.AlwaysUseDefaultsCheckBox);
			alwaysUseDefaultCheckBox.Checked = ViewModel.AlwaysUseDefaults;
			alwaysUseDefaultCheckBox.CheckedChange+= (sender, e) => { ViewModel.AlwaysUseDefaults = e.IsChecked;};
        }
    }
}