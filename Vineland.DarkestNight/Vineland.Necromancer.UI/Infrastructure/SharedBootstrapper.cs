using System;
using System.Collections.Generic;
using System.Text;
using Vineland.Necromancer.Core;
using Vineland.DarkestNight.UI.Services;
using Vineland.Necromancer.UI;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Ioc.SimpleInjectorContainer;
using SimpleInjector;
using System.Runtime.InteropServices;
using Vineland.Necromancer.Repository;
using Vineland.Necromancer.Core.Services;
using Plugin.Toasts;

namespace Vineland.DarkestNight.UI.Infrastructure
{
    public abstract class SharedBootstrapper
    {
        public SharedBootstrapper()
        {
        }
		public void Run(){
			SetIoC();
		}

        protected void SetIoC()
		{
			if (Resolver.IsSet)
				return;
			
			var container = new Container();

			//there can be only one
			container.RegisterSingleton<NavigationService>();
			//singletons because they are dependecies for the above singleton
			container.RegisterSingleton<PageService>();

			container.Register<IRepository, EmbeddedResourceRepository>();
			container.Register<IFileService, FileService>();
			//container.Register<IToastNotificator, ToastNotification>();

			RegisterPlatformSpecificImplementations (container);

			Resolver.SetResolver (new SimpleInjectorResolver (container));
        }

		protected abstract void RegisterPlatformSpecificImplementations(Container container);
    }
}
