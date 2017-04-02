using System;
using System.Threading.Tasks;
using FreshMvvm;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace Vineland.Necromancer.UI
{
	public static class FreshMvvmExtensions
	{
		public static async Task PushPopup<T>(this IPageModelCoreMethods coreMethods, object data = null, T pageModel = null, bool animate = true) where T : FreshBasePageModel
		{
			if(pageModel == null)
				pageModel = FreshIOC.Container.Resolve<T>();
			
            var page = FreshPageModelResolver.ResolvePageModel(data, pageModel);
			if (page is PopupPage)
			{
				await PopupNavigation.PushAsync(page as PopupPage, animate);
			}
		}

		public static async Task PopPopup(this IPageModelCoreMethods coreMethods, bool animate = true)
		{
			await PopupNavigation.PopAsync(animate);
		}
	}
}
