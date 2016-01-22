using System;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight;
using Android.Util;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI
{
	public class OptionsViewModel : BaseViewModel
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

        public DarknessCardsMode[] DarknessCardsModeOptions { get; private set; }
    }
}
