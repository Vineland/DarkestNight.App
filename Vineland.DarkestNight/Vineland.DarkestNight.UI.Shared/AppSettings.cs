using System;
using System.Collections.Generic;
using System.Text;
using Vineland.DarkestNight.Core;
using Vineland.DarkestNight.UI.Shared.Services;

namespace Vineland.DarkestNight.UI.Shared
{
    public class AppSettings
    {
        ISettingsService _settingsService;

        public AppSettings(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public bool PallOfSuffering
        {
            get { return _settingsService.LoadBoolean("PallOfSuffering"); }
            set { _settingsService.SaveBoolean("PallOfSuffering", value); }
        }

        public DarknessCardsMode DarknessCardsMode
        {
            get { return (DarknessCardsMode)_settingsService.LoadInt("DarknessCardsMode"); }
            set { _settingsService.SaveInt("DarknessCardsMode", (int)value); }
        }
    }
}
