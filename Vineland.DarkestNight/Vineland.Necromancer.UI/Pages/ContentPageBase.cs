using System;
using Android.Nfc;
using GalaSoft.MvvmLight;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using XLabs.Ioc;
using System.Runtime.InteropServices;
using Vineland.Necromancer.UI;
using System.Dynamic;

namespace Vineland.Necromancer.UI
{
	public abstract class ContentPageBase<T> : ContentPage where T: ViewModelBase
	{
		public T ViewModel { get; set; }
		public NavigationService NavigationService { get; set; }

		public ContentPageBase ()
		{
			ViewModel = Resolver.Resolve<T> ();
			NavigationService = Resolver.Resolve<NavigationService> ();
			BindingContext = ViewModel;
			BackgroundImage = "background.png";
		}
	}
}

