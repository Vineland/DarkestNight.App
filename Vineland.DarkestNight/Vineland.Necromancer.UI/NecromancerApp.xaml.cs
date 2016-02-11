using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using Android.Animation;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Vineland.Necromancer.UI
{
	public partial class NecromancerApp : Application
	{
		public NecromancerApp ()
		{
			InitializeComponent ();

			MainPage = new NavigationPage (Resolver.Resolve<PageService> ().CreatePage<HomePage> ());

			Resolver.Resolve<NavigationService> ().SetNavigation (MainPage.Navigation);

			var buttonStyle = new Style (typeof(Button));
			buttonStyle.Setters.Add (new Setter () {
				Property = Button.BackgroundColorProperty,
				Value = new Color ((double)102/255, (double)82/255, (double)54/255, 0.75)
			});
			buttonStyle.Setters.Add (new Setter () {
				Property = Button.TextColorProperty,
				Value = Color.White//new Color ((double)216 / 255, (double)198 / 255, (double)152 / 255)
			});
			buttonStyle.Setters.Add (new Setter () {
				Property = Button.FontSizeProperty,
				Value = Device.GetNamedSize(NamedSize.Large, typeof(Button))
			});
			Resources = new ResourceDictionary ();
			Resources.Add (buttonStyle);
		
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

