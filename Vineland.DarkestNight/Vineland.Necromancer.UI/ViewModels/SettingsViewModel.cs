using System;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class SettingsViewModel : BaseViewModel
	{
		public SettingsViewModel (Settings settings)
		{
			Settings = settings;
		}

		public Settings Settings {get; private set;}
	}
}

