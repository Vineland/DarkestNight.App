using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Vineland.DarkestNight.UI.Shared.ViewModels;
using Vineland.DarkestNight.UI.Shared;

namespace Vineland.DarkestNight.UI.Android
{
    [Activity(MainLauncher = true)]
    public class HomeActivity : Activity
    {
        public HomeViewModel ViewModel { get; set; }

		public HomeActivity ()
		{
			IoC.BuildUp(this);
		}

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ViewModel.Initialise();

            SetContentView(Resource.Layout.Home);

            var continueButton = FindViewById<Button>(Resource.Id.ContinueButton);
            continueButton.Visibility = ViewModel.CanContinue ? ViewStates.Visible : ViewStates.Gone;
            continueButton.Click += ContinueButton_Click;

            FindViewById<Button>(Resource.Id.NewGameButton).Click += NewGameButton_Click;

            var loadGameButton = FindViewById<Button>(Resource.Id.LoadGameButton);
            loadGameButton.Click += LoadGameButton_Click;
            loadGameButton.Enabled = ViewModel.CanLoad;

            FindViewById<Button>(Resource.Id.OptionsButton).Click += OptionsButton_Click;

            FindViewById<Button>(Resource.Id.CreditsButton).Click += CreditsButton_Click;
        }

        private void CreditsButton_Click(object sender, EventArgs e)
        {
            
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
			//StartActivity(typeof(OptionsActivity));
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
			StartActivity(typeof(NewGameActivity));
        }
    }
}

