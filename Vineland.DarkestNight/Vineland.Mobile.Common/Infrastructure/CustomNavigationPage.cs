using System;
using Xamarin.Forms;

namespace Vineland.Xamarin.Forms.Common
{
	public class CustomNavigationPage: NavigationPage
	{
		public CustomNavigationPage(Page content)
			: base(content)
		{
			Init();
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

