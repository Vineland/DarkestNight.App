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
		/// Key: viewmodel name 
		/// Value: page type
		/// </summary>
		Dictionary<string, Type> _viewModelPageMappings;

		public PageService ()
		{
			var type = typeof(Page);
			_viewModelPageMappings = AppDomain.CurrentDomain.GetAssemblies ()
				.SelectMany (s => s.GetTypes ())
				.Where (p => type.IsAssignableFrom (p))
				.ToDictionary (x => x.Name + "ViewModel", x => x);
		}

		public Page CreatePage<T>() where T:class
		{
			var pageType = _viewModelPageMappings [typeof(T).Name];
			var constructor = pageType.GetTypeInfo()
				.DeclaredConstructors
				.FirstOrDefault(c => !c.GetParameters().Any());
			var page = constructor.Invoke (new object[] { }) as Page;

				//Activator.CreateInstance(_viewModelPageMappings [typeof(T).Name]) as Page;
			page.BindingContext = Resolver.Resolve<T>();

			return page;
		}

		public Type GetPageType<T>()
		{
			return _viewModelPageMappings [typeof(T).Name];			
		}
	}
}

