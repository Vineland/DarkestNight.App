using System;
using Vineland.DarkestNight.Core;

namespace Vineland.DarkestNight.UI.Shared.ViewModels
{
    public class OptionsViewModel
    {

        AppSettings _appSettings;

        public OptionsViewModel(AppSettings appSettings)
        {
            _appSettings = appSettings;
            DarknessCardsModeOptions = (DarknessCardsMode[])Enum.GetValues(typeof(DarknessCardsMode));
        }

        public bool PallOfSuffering { get; set; }
        public DarknessCardsMode DarknessCardsMode { get; set; }
        public int StartingDarkness { get; set; }

        public DarknessCardsMode[] DarknessCardsModeOptions { get; private set; }

        public void Initalise()
        {
            PallOfSuffering = _appSettings.PallOfSuffering;
            DarknessCardsMode = _appSettings.DarknessCardsMode;
            StartingDarkness = _appSettings.StartingDarkness;
        }

        public void UpdateSettings()
        {
            _appSettings.PallOfSuffering = PallOfSuffering;
            _appSettings.DarknessCardsMode = DarknessCardsMode;
            _appSettings.StartingDarkness = StartingDarkness;
        }
    }
}
