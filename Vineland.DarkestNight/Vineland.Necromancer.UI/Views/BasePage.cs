using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class BasePage: ContentPage
	{
		public BasePage ()
		{
		}

		protected override void OnAppearing ()
		{
			var viewModel = BindingContext as BaseViewModel;
			if(viewModel != null)
				viewModel.OnAppearing ();

			base.OnAppearing ();
		}

		protected override void OnDisappearing ()
		{
			var viewModel = BindingContext as BaseViewModel;
			if (viewModel != null)
				viewModel.OnDisappearing ();
			
			base.OnDisappearing ();
		}

		protected override bool OnBackButtonPressed ()
		{
			var viewModel = BindingContext as BaseViewModel;
			if (viewModel != null)
				viewModel.OnBackButtonPressed ();
			
			return base.OnBackButtonPressed ();
		}
	}

	public class BaseTabbedPage: TabbedPage
	{
		public BaseTabbedPage ()
		{
		}

		protected override void OnAppearing ()
		{
			var viewModel = BindingContext as BaseViewModel;
			if(viewModel != null)
				viewModel.OnAppearing ();

			base.OnAppearing ();
		}

		protected override void OnDisappearing ()
		{
			var viewModel = BindingContext as BaseViewModel;
			if (viewModel != null)
				viewModel.OnDisappearing ();

			base.OnDisappearing ();
		}

		protected override bool OnBackButtonPressed ()
		{
			var viewModel = BindingContext as BaseViewModel;
			if (viewModel != null)
				viewModel.OnBackButtonPressed ();

			return base.OnBackButtonPressed ();
		}
	}
}

