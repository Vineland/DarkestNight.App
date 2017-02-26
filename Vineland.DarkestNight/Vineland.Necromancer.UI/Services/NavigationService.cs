using System;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using XLabs.Ioc;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

namespace Vineland.Necromancer.UI
{
	public class NavigationService
	{
		INavigation _navigation;
		PageService _pageService;

		public NavigationService (PageService pageService)
		{
			_pageService = pageService;
		}

		public void SetNavigation (INavigation navigation)
		{
			_navigation = navigation;

		}

		public async void Pop ()
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");
			
			await _navigation.PopAsync ();
		}

		public async void PopToRoot ()
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");
			
			await _navigation.PopToRootAsync ();
		}

		public async void PopTo<T> () where T: Page
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");
			//TODO: needs to be more robust
			//remove all pages between the current page and target page
			for (int i = _navigation.NavigationStack.Count - 2; i >= 0; i--) {
				if (_navigation.NavigationStack [i].GetType () == typeof(T))
					break;
				_navigation.RemovePage (_navigation.NavigationStack [i]);
			}
			//pop the current page
			await _navigation.PopAsync ();
		}

		public async Task<Page> Push<T> (object viewModel = null, bool clearBackStack = false) where T : Page
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");

			try {
				var newPage = _pageService.CreatePage<T> (viewModel);
				await _navigation.PushAsync (newPage as Page);

				if (clearBackStack) {
					//remove all pages between the root page and target page
					var pagesToRemove = new List<Page> ();
					for (int i = 1; i < _navigation.NavigationStack.Count - 1; i++)
						pagesToRemove.Add (_navigation.NavigationStack [i]);

					foreach (var page in pagesToRemove)
						_navigation.RemovePage (page);
				}

				return newPage;

			} catch (Exception ex) {
				LogHelper.Error(ex);
				throw;
			}
		}

		public async void PushPopup<T>(object viewModel = null) where T : PopupPage
		{
			if (_navigation == null)
				throw new Exception("_navigation is null");
			try
			{
				var newPage = _pageService.CreatePage<T>(viewModel);
				await _navigation.PushPopupAsync(newPage as PopupPage);
			}
			catch (Exception ex)
			{
				LogHelper.Error(ex);
				throw;
			}
		}

		public async void PopLastPopup()
		{
			if (_navigation == null)
				throw new Exception("_navigation is null");
			try
			{
			await _navigation.PopPopupAsync();
				}
			catch (Exception ex)
			{
				LogHelper.Error(ex);
				throw;
			}
		}

		public async void PushModal<T> (object viewModel = null) where T : Page
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");

			try {
				var newPage = _pageService.CreatePage<T> (viewModel);
				await _navigation.PushModalAsync (newPage as Page);
			} catch (Exception ex) {
				LogHelper.Error(ex);
				throw;
			}
		}


		public async void PopModal ()
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");

			await _navigation.PopModalAsync ();
		}

		public async void DisplayAlert(string title, string message, string cancel){

			if (_navigation == null)
				throw new Exception ("_navigation is null");

			await _navigation.NavigationStack.Last().DisplayAlert(title, message, cancel);
		}

		public async Task<bool> DisplayConfirmation(string title, string message, string accept, string cancel){

			if (_navigation == null)
				throw new Exception ("_navigation is null");

			var result = await _navigation.NavigationStack.Last().DisplayAlert(title, message, accept, cancel);

			return result;
		}

		public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons){

			if (_navigation == null)
				throw new Exception ("_navigation is null");

			var result = await _navigation.NavigationStack.Last().DisplayActionSheet(title, cancel, destruction, buttons);

			return result;
		}
	}
}

