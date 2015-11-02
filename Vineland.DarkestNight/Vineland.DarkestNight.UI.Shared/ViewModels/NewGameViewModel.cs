using System;
using System.Collections.Generic;
using System.Text;
using Vineland.DarkestNight.UI.Shared.Services;

namespace Vineland.DarkestNight.UI.Shared.ViewModels
{
    public class NewGameViewModel
    {
        ISettingsService _settings;

        public NewGameViewModel(ISettingsService settings)
        {
            _settings = settings;
        }


        public void Initialise()
        {
            //load defaults from settings
        }

        public bool PallOfSuffering { get; set; }
    }
}
