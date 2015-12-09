using System;
using Android.Nfc;
using GalaSoft.MvvmLight;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI
{
	public class PageBase<T> : ContentPage where T: ViewModelBase
	{
		public T ViewModel { get; set; }

		public PageBase ()
		{
			IoC.BuildUp (this);
			BindingContext = ViewModel;
		}
	}
}

