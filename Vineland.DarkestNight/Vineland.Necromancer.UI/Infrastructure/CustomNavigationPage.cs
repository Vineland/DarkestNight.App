using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class CustomNavigationPage: NavigationPage
	{
		public CustomNavigationPage(Page content)
			: base(content)
		{
			Init();

			BarBackgroundColor = Color.FromHex("#BC63402D");
			BarTextColor = Color.White;
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

