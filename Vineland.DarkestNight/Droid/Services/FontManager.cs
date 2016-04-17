using System;
using Android.Graphics;
using Android.Content;
using Java.Util;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI.Droid
{
	public static class FontManager
	{
		public static string DefaultFontName = "baskervillebecker";

		private static Dictionary<string, Typeface> _fonts = new Dictionary<string, Typeface> ();

		public static Typeface GetDefaultFont ()
		{
			return GetFont (DefaultFontName);
		}
		public static Typeface GetFont (string name)
		{
			if (string.IsNullOrEmpty (name))
				return GetDefaultFont ();

			if (!_fonts.ContainsKey (name)) {
				try 
				{
					_fonts.Add (name, Typeface.CreateFromAsset (Forms.Context.Assets, name + ".ttf"));

				} catch (Exception ex) {
					LogHelper.Error (ex);
					return GetDefaultFont ();
				}
			}
			return _fonts [name];

			 
		}

	}
}

