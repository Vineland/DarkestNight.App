using System;
using Vineland.DarkestNight.Core;
using GalaSoft.MvvmLight;

namespace Vineland.DarkestNight.UI.ViewModels
{
	public class OptionsViewModel :ViewModelBase
    {
        AppSettings _appSettings;

        public OptionsViewModel(AppSettings appSettings)
        {
            _appSettings = appSettings;
            DarknessCardsModeOptions = (DarknessCardsMode[])Enum.GetValues(typeof(DarknessCardsMode));
        }

		public bool PallOfSuffering {
			get { return _appSettings.PallOfSuffering;}
			set{ _appSettings.PallOfSuffering = value;}
		}
		public DarknessCardsMode DarknessCardsMode {
			get { return _appSettings.DarknessCardsMode;}
			set{ _appSettings.DarknessCardsMode = value;}
		}
		public string StartingDarkness {
			get{ return _appSettings.StartingDarkness.ToString();}
			set{ _appSettings.StartingDarkness = int.Parse(value);}
		}
		public bool AlwaysUseDefaults {
			get { return _appSettings.AlwaysUseDefaults;}
			set{ _appSettings.AlwaysUseDefaults = value;}
		}

        public DarknessCardsMode[] DarknessCardsModeOptions { get; private set; }
    }
}
