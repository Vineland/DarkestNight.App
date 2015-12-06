using System;

using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using Vineland.DarkestNight.UI.ViewModels;

namespace Vineland.Necromancer.UI
{
	public class App : Application
	{
		public App ()
		{
			MainPage = new NavigationPage(new Home());

			IoC.Get<NavigationService> ().SetNavigation (MainPage.Navigation);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

