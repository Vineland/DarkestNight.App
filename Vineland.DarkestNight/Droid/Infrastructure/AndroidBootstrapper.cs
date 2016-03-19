using Vineland.DarkestNight.UI.Droid.Services;
using Vineland.DarkestNight.UI.Infrastructure;
using Vineland.DarkestNight.UI.Services;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;

namespace Vineland.DarkestNight.UI.Droid.Infrastructure
{
	public class AndroidBootstrapper: SharedBootstrapper
    {
		protected override void RegisterPlatformSpecificImplementations (SimpleInjector.Container container)
		{
			container.Register<ISettingsService, SettingsService>();
			container.Register<IDisplayAlertService, DisplayAlertService> ();
		}
    }
}