using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public static class ImageSourceUtil
	{
		private static ImageSource GetImageSource(string rawName, string typePrefix)
		{
			if (string.IsNullOrEmpty (rawName))
				return null;
			
			var name = rawName.Replace (" ", "_").Replace ("'", " ").ToLower ();
			if (string.IsNullOrEmpty (typePrefix))
				return ImageSource.FromFile (name);
			
			return ImageSource.FromFile (string.Format ("{0}_{1}", typePrefix, name));	
		}

		public static ImageSource GetHeroImage(string name){
			return GetImageSource (name, "");
		}

		public static ImageSource GetItemImage(string name){
			return GetImageSource (name, "item");
		}

		public static ImageSource GetBlightImage(string name){
			return GetImageSource (name, "blight");
		}
	}
}

