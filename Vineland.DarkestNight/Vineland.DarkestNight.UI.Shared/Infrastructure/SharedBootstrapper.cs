using System;
using System.Collections.Generic;
using System.Text;
using Vineland.DarkestNight.Core;
using Vineland.DarkestNight.UI.Shared.ViewModels;

namespace Vineland.DarkestNight.UI.Shared.Infrastructure
{
    public abstract class SharedBootstrapper
    {
        public SharedBootstrapper()
        {
        }

        public virtual void Register()
        {
            TinyIoC.TinyIoCContainer.Current.Register<AppSettings>();
            TinyIoC.TinyIoCContainer.Current.Register<GameState>().AsSingleton();

            TinyIoC.TinyIoCContainer.Current.Register<HomeViewModel>();
            TinyIoC.TinyIoCContainer.Current.Register<NewGameViewModel>();
            TinyIoC.TinyIoCContainer.Current.Register<OptionsViewModel>();
        }
    }
}
