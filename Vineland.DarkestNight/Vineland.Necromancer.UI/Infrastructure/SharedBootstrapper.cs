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

			container.RegisterSingleton<GameStateService>();
			container.RegisterSingleton<FileService>();
			container.RegisterSingleton<PageService>();
			container.RegisterSingleton<NavigationService>();
			container.RegisterSingleton<DataService>();

			RegisterPlatformSpecificImplementations (container);

			Resolver.SetResolver (new SimpleInjectorResolver (container));
        }

		protected abstract void RegisterPlatformSpecificImplementations(Container container);
    }
}
