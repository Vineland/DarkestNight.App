using System;
using System.Collections.Generic;
using System.Text;

namespace Vineland.DarkestNight.UI.Shared
{
    public static class AppConstants
    {
        public static string AppName = "Necromancer App";
        public static string SaveFileExtension = ".sav";

        public static string SavesLocation
        {
            get { 
				return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
			}
        }

    }
}
