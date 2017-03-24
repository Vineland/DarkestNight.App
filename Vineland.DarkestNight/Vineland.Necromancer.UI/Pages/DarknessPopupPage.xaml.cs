using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class DarknessPopupPage : PopupPage
	{

		public DarknessPopupPage()
		{
			InitializeComponent();
			CloseWhenBackgroundIsClicked = true;
			LayoutRoot.BackgroundColor = AppConstants.PopupBackground;
		}

		protected override void OnDisappearing()
		{
			var viewModel = BindingContext as BaseViewModel;
			if (viewModel != null)
				viewModel.OnDisappearing();

			base.OnDisappearing();
		}
	}
}
