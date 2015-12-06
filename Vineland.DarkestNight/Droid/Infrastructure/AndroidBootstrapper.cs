using Vineland.DarkestNight.UI.Droid.Services;
using Vineland.DarkestNight.UI.Infrastructure;
using Vineland.DarkestNight.UI.Services;

namespace Vineland.DarkestNight.UI.Droid.Infrastructure
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