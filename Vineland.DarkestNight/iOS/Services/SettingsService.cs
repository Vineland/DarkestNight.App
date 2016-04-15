using System;
using Vineland.Necromancer.Core;
using Foundation;

namespace Vineland.Necromancer.iOS
{
	public class SettingsService : ISettingsService
	{
		public SettingsService ()
		{
		}


		public string LoadString (string settingName, string @default = null)
		{
			return NSUserDefaults.StandardUserDefaults.StringForKey (settingName);
		}

		public bool LoadBoolean (string settingName, bool @default = false)
		{
			return NSUserDefaults.StandardUserDefaults.BoolForKey (settingName);
		}

		public int LoadInt (string settingName, int @default = 0)
		{
			return (int) NSUserDefaults.StandardUserDefaults.IntForKey (settingName);
		}

		public void SaveString (string settingName, string value)
		{
			NSUserDefaults.StandardUserDefaults.SetString (value, settingName);
		}

		public void SaveBoolean (string settingName, bool value)
		{
			NSUserDefaults.StandardUserDefaults.SetBool (value, settingName);
		}

		public void SaveInt (string settingName, int value)
		{
			NSUserDefaults.StandardUserDefaults.SetInt (value, settingName);
		}

	}
}

