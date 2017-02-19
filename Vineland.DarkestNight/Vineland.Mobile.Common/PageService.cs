using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using XLabs.Ioc;
using System.Reflection;

namespace Vineland.Xamarin.Forms.Common
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
			var currentDomain = typeof(string).GetTypeInfo().Assembly.GetType("AppDomain").GetRuntimeProperty("CurrentDomain").GetMethod.Invoke(null, new object[] { });
			var getAssemblies = currentDomain.GetType().GetRuntimeMethod("GetAssemblies", new Type[] { });
			var assemblies = getAssemblies.Invoke(currentDomain, new object[] { }) as Assembly[];
			_viewModelPageMappings = assemblies
				.SelectMany (s => s.DefinedTypes)
				.Where (p => p.Name.EndsWith("ViewModel", StringComparison.CurrentCulture))
				.ToDictionary (x => x.Name.Replace("ViewModel", "Page"), x => x.GetType());
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

