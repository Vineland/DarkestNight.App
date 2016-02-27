using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using Android.Animation;
using Vineland.Necromancer.Core;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Vineland.DarkestNight.UI;
using Vineland.DarkestNight.UI.Services;
using Android.Views.InputMethods;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Vineland.Necromancer.UI
{
	public partial class NecromancerApp : Application
	{
		public FileService FileService { get; private set; }

		public NavigationService Navigation { get; private set; }

		public NecromancerApp ()
		{
			InitializeComponent ();

			FileService = Resolver.Resolve<FileService> ();
			
			MainPage = new NavigationPage (Resolver.Resolve<PageService> ().CreatePage<HomePage> ());

			Navigation = Resolver.Resolve<NavigationService> ();
			Navigation.SetNavigation (MainPage.Navigation);

		}

		public GameState CurrentGame { get; set; }

		public async void SaveCurrentGame ()
		{
			await Task.Run (() => {
				FileService.SaveFile (AppConstants.SaveFilePath, JsonConvert.SerializeObject (CurrentGame));
			});
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

