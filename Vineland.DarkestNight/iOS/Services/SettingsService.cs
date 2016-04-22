using System;
using Vineland.Necromancer.Core;
using Foundation;

namespace Vineland.Necromancer.iOS
{
	public class SettingsService : ISettingsService
	{
		private NSUserDefaults Defaults
		{
			get
			{
				var d = NSUserDefaults.StandardUserDefaults;
				d.Synchronize();
				return d;
			}
		}

		public SettingsService ()
		{
		}


		public string LoadString (string settingName, string @default = null)
		{
			if(Defaults.ValueForKey(new NSString(settingName)) == null)
				return @default;
			
			return Defaults.StringForKey (settingName);
		}

		public bool LoadBoolean (string settingName, bool @default = false)
		{
			if(Defaults.ValueForKey(new NSString(settingName)) == null)
				return @default;
			
			return Defaults.BoolForKey (settingName);
		}

		public int LoadInt (string settingName, int @default = 0)
		{
			if(Defaults.ValueForKey(new NSString(settingName)) == null)
				return @default;
			
			return (int) Defaults.IntForKey (settingName);
		}

		public void SaveString (string settingName, string value)
		{
			Defaults.SetString (value, settingName);
			Defaults.Synchronize ();
		}

		public void SaveBoolean (string settingName, bool value)
		{
			Defaults.SetBool (value, settingName);
			Defaults.Synchronize ();
		}

		public void SaveInt (string settingName, int value)
		{
			Defaults.SetInt (value, settingName);
			Defaults.Synchronize ();
		}

	}
}

