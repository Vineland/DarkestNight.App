using System;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Vineland.Necromancer.UI
{
	public class NavigationService
	{
		INavigation _navigation;

		/// <summary>
		/// The key is the viewmodel name and the value is the view type
		/// </summary>
		Dictionary<string, Type> _viewModelPageMappings;

		public NavigationService ()
		{
			var type = typeof(Page);
			_viewModelPageMappings = AppDomain.CurrentDomain.GetAssemblies ()
				.SelectMany (s => s.GetTypes ())
				.Where (p => type.IsAssignableFrom (p))
				.ToDictionary (x => x.Name + "ViewModel", x => x);
		}

		public void SetNavigation (INavigation navigation)
		{
			_navigation = navigation;

		}

		public void Pop ()
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");
			
			_navigation.PopAsync ();
		}

		public void PopToRoot ()
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");

			_navigation.PopToRootAsync ();
		}

		public void PopToViewModel<T> ()
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");
			//TODO: needs to be more robust
			var pageType = _viewModelPageMappings [typeof(T).Name];
			//remove all pages between the current page and target page
			for (int i = _navigation.NavigationStack.Count - 2; i >= 0; i--) {
				if (_navigation.NavigationStack [i].GetType () == pageType)
					break;
				_navigation.RemovePage (_navigation.NavigationStack [i]);
			}
			//pop the current page
			_navigation.PopAsync ();
		}

		public async void PushViewModel<T> (bool clearBackStack = false)
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");

			try {
				var page = Activator.CreateInstance (_viewModelPageMappings [typeof(T).Name]);

					await _navigation.PushAsync (page as Page);
					if(clearBackStack){

						//remove all pages between the current page and target page
						for (int i = 1; i < _navigation.NavigationStack.Count - 1; i++) {
							_navigation.RemovePage (_navigation.NavigationStack [i]);
						}
					}
			} catch (Exception ex) {
				throw ex;
			}
		}
	}
}

