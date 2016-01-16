using System;
using Vineland.DarkestNight.Core;
using GalaSoft.MvvmLight;
using Android.Util;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI
{
	public class OptionsViewModel :ViewModelBase
    {
        AppSettings _appSettings;

        public OptionsViewModel(AppSettings appSettings)
        {
            _appSettings = appSettings;
            DarknessCardsModeOptions = (DarknessCardsMode[])Enum.GetValues(typeof(DarknessCardsMode));
        }

		public AppSettings Settings{
			get { return _appSettings; }
		}

		public bool PallOfSuffering {
			get { return _appSettings.PallOfSuffering;}
			set{ _appSettings.PallOfSuffering = value;}
		}

		public DarknessCardsMode DarknessCardsMode {
			get { return _appSettings.DarknessCardsMode;}
			set{ 
				if (_appSettings.DarknessCardsMode == value)
					return;
				
				_appSettings.DarknessCardsMode = value;
				RaisePropertyChanged(()=> DarknessCardsMode);
			}
		}
		public string StartingDarkness {
			get{ return _appSettings.StartingDarkness.ToString();}
			set{ 
				_appSettings.StartingDarkness = int.Parse(value);
			
			}
		}
		public bool AlwaysUseDefaults {
			get { return _appSettings.AlwaysUseDefaults;}
			set{ _appSettings.AlwaysUseDefaults = value;}
		}

        public DarknessCardsMode[] DarknessCardsModeOptions { get; private set; }
    }
}
