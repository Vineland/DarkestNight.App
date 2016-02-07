using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Vineland.DarkestNight.UI
{
    public static class AppConstants
    {
        public static string AppName = "Necromancer App";

        public static string SaveFilePath
        {
            get { 
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "savegame.sav"); 
			}
        }

    }
}
