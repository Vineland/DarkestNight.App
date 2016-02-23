using System;
using System.Collections.Generic;
using System.Text;

namespace Vineland.Necromancer.Core
{
    public interface ISettingsService
    {
        string LoadString(string settingName, string @default = null);
        bool LoadBoolean(string settingName, bool @default = false);
        int LoadInt(string settingName, int @default = 0);

        void SaveString(string settingName, string value);
        void SaveBoolean(string settingName, bool value);
        void SaveInt(string settingName, int value);
    }
}
