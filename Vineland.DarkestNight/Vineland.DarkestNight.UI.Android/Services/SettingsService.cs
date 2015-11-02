using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Vineland.DarkestNight.UI.Shared.Services;

namespace Vineland.DarkestNight.UI.Android.Services
{
    public class SettingsService : ISettingsService
    {
        ISharedPreferences _sharedPreferences;
        ISharedPreferencesEditor _sharedPreferencesEditor;

        public SettingsService(string appName)
        {
            _sharedPreferences = Application.Context.GetSharedPreferences(appName, FileCreationMode.Private);
            _sharedPreferencesEditor = _sharedPreferences.Edit();
        }

        public bool LoadBoolean(string settingName, bool @default = false)
        {
            return _sharedPreferences.GetBoolean(settingName, @default);
        }

        public int LoadInt(string settingName, int @default = 0)
        {
            return _sharedPreferences.GetInt(settingName, @default);
        }

        public string LoadString(string settingName, string @default = null)
        {
            return _sharedPreferences.GetString(settingName, @default);
        }

        public void SaveBoolean(string settingName, bool value)
        {
            _sharedPreferencesEditor.PutBoolean(settingName, value);
        }

        public void SaveInt(string settingName, int value)
        {
            _sharedPreferencesEditor.PutInt(settingName, value);
        }

        public void SaveString(string settingName, string value)
        {
            _sharedPreferencesEditor.PutString(settingName, value);
        }
    }
}