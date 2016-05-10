using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
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

		#region Colours

		public static Color ButtonBackground = Color.FromHex("#78FFFCCC");
		public static Color HeaderBackground = Color.FromHex ("#BC63402D");
		public static Color NavBarBackground = Color.FromHex ("#BC63402D");
		public static Color SeparatorColour = Color.FromHex("#BC63402D");
		public static Color TextColour = Color.Black;
		public static Color HeaderTextColour = Color.White;
		public static Color NavBarTextColour = Color.White;

		#endregion
    }
}
