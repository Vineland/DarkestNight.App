using System;
using System.Collections.Generic;
using System.Text;
using Vineland.DarkestNight.UI.Shared.Services;
using Vineland.DarkestNight.Core;

namespace Vineland.DarkestNight.UI.Shared
{
    public class AppSettings
    {

        private const string DARKNESS_CARDS_MODE = "DarknessCardsMode";
        private const string PALL_OF_SUFFERING = "PallOfSuffering";
        private const string STARTING_DARKNESS = "StartingDarkness";
		private const string PROMPT_ON_NEW_GAME="PromptOnNewGame";

        ISettingsService _settingsService;

        public AppSettings(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public bool PromptOnNewGame
        {
			get { return _settingsService.LoadBoolean(PROMPT_ON_NEW_GAME); }
			set { _settingsService.SaveBoolean(PROMPT_ON_NEW_GAME, value); }
        }

		public int StartingDarkness
		{
			get { return _settingsService.LoadInt(STARTING_DARKNESS); }
			set { _settingsService.SaveInt(STARTING_DARKNESS, value); }
		}

        public bool PallOfSuffering
        {
            get { return _settingsService.LoadBoolean(PALL_OF_SUFFERING); }
            set { _settingsService.SaveBoolean(PALL_OF_SUFFERING, value); }
        }

        public DarknessCardsMode DarknessCardsMode
        {
            get { return (DarknessCardsMode)_settingsService.LoadInt(DARKNESS_CARDS_MODE); }
            set { _settingsService.SaveInt(DARKNESS_CARDS_MODE, (int)value); }
        }
    }
}
