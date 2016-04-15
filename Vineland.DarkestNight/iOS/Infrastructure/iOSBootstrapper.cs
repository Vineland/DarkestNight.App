using System;
using Vineland.DarkestNight.UI.Infrastructure;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.iOS
{
	public class iOSBootstrapper :SharedBootstrapper
	{
		protected override void RegisterPlatformSpecificImplementations (SimpleInjector.Container container)
		{
			container.Register<ISettingsService, SettingsService>();
		}
	}
}

