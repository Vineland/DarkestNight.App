using System;
using Android.Nfc;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using XLabs.Ioc;
using System.Reflection;
using Android.Systems;

namespace Vineland.Necromancer.UI
{
	public class PageService
	{
		/// <summary>
		/// Key: page name 
		/// Value: view model type
		/// </summary>
		Dictionary<string, Type> _viewModelPageMappings;

		public PageService ()
		{
			var type = typeof(BaseViewModel);
			_viewModelPageMappings = AppDomain.CurrentDomain.GetAssemblies ()
				.SelectMany (s => s.GetTypes ())
				.Where (p => type.IsAssignableFrom (p))
				.ToDictionary (x => x.Name.Replace("ViewModel", "Page"), x => x);
		}

		public Page CreatePage<T>(object viewModel = null) where T: Page
		{
			var page = Resolver.Resolve<T>();
			if (viewModel == null) 
			{
				var viewModelType = _viewModelPageMappings [typeof(T).Name];
				viewModel = Resolver.Resolve (viewModelType);
			}

			page.BindingContext = viewModel;

			return page;
		}

	}
}

