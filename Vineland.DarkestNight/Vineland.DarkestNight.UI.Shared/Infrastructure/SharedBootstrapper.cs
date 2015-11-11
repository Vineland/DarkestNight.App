using System;
using System.Collections.Generic;
using System.Text;
using Vineland.DarkestNight.Core;
using Vineland.DarkestNight.UI.Shared.ViewModels;
using Vineland.DarkestNight.UI.Shared.Services;

namespace Vineland.DarkestNight.UI.Shared.Infrastructure
{
    public abstract class SharedBootstrapper
    {
        public SharedBootstrapper()
        {
        }
		public void Run(){
			RegisterTypes();
		}
        protected virtual void RegisterTypes()
		{
			IoC.SetContainer(TinyIoC.TinyIoCContainer.Current);

            TinyIoC.TinyIoCContainer.Current.Register<AppSettings>();
			TinyIoC.TinyIoCContainer.Current.Register<FileService>();
			TinyIoC.TinyIoCContainer.Current.Register<AppGameState>().AsSingleton();

            TinyIoC.TinyIoCContainer.Current.Register<HomeViewModel>();
            TinyIoC.TinyIoCContainer.Current.Register<NewGameViewModel>();
            TinyIoC.TinyIoCContainer.Current.Register<OptionsViewModel>();
        }
    }
}
