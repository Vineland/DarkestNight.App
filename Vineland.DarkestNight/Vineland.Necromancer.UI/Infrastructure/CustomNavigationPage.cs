using System;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI
{
	public class CustomNavigationPage: NavigationPage
	{
		public CustomNavigationPage(Page content)
			: base(content)
		{
			Init();
			BarBackgroundColor = AppConstants.NavBarBackground;
			BarTextColor = AppConstants.NavBarTextColour;
		}

		private void Init()
		{
//			this.Pushed += (object sender, NavigationEventArgs e) => {
//				var viewModel = e.Page.BindingContext as BaseViewModel;
//				if (viewModel != null)
//					viewModel.OnAppearing();
//			};
			this.Popped += (object sender, NavigationEventArgs e) =>
			{
				var viewModel = e.Page.BindingContext as BaseViewModel;
				if (viewModel != null)
					viewModel.Cleanup();
			};
		}
	}
}

