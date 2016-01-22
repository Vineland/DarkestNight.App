using System;

using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using XLabs.Ioc;
using Vineland.Necromancer.Core;
using Xamarin.Forms.Xaml;

namespace Vineland.Necromancer.UI
{
	public class NecromancerApp : Application
	{
		public NecromancerApp ()
		{
			var mainPage = new NavigationPage(Resolver.Resolve<PageService>().CreatePage<HomeViewModel>());
			mainPage.BarBackgroundColor = Color.FromHex ("#52361b");
			mainPage.BarTextColor = Color.White;
			MainPage = mainPage;

			Resolver.Resolve<NavigationService> ().SetNavigation (MainPage.Navigation);
		}

		public GameState CurrentGame{ get; set; }

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

