using Vineland.DarkestNight.UI.Android.Services;
using Vineland.DarkestNight.UI.Shared.Infrastructure;
using Vineland.DarkestNight.UI.Shared.Services;

namespace Vineland.DarkestNight.UI.Android.Infrastructure
{
    public class AndroidBootstrapper: SharedBootstrapper
    {
        protected override void RegisterTypes()
        {
            base.RegisterTypes();

            TinyIoC.TinyIoCContainer.Current.Register<ISettingsService, SettingsService>();
        }
    }
}