using System;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vineland.Necromancer.UI
{
	public class NavigationService 
	{
		INavigation _navigation;
		IEnumerable<Type> _pageTypes;

		/// <summary>
		/// The key is the viewmodel name and the value is the view type
		/// </summary>
		Dictionary<string, Type> _viewModelPageMappings;

		public NavigationService ()
		{
			var type = typeof(ContentPage);
			_viewModelPageMappings = AppDomain.CurrentDomain.GetAssemblies ()
				.SelectMany (s => s.GetTypes ())
				.Where (p => type.IsAssignableFrom (p))
				.ToDictionary (x => x.Name + "ViewModel", x => x);
		}

		public void SetNavigation(INavigation navigation){
			_navigation = navigation;
		}

		public void GoBack ()
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");

			_navigation.PopAsync ();
		}

		public void GoToRoot(){
			if (_navigation == null)
				throw new Exception ("_navigation is null");

			_navigation.PopToRootAsync();
		}

		public void NavigateToViewModel<T>()
		{
			if (_navigation == null)
				throw new Exception ("_navigation is null");
			
			var page = Activator.CreateInstance (_viewModelPageMappings[typeof(T).Name]);

			_navigation.PushAsync (page as Page);

		}
	}
}

