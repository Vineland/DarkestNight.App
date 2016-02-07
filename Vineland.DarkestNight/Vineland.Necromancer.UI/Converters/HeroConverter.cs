using System;
using Xamarin.Forms;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class HeroConverter : IValueConverter
	{
		#region IValueConverter implementation

		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var hero = value as Hero;
			if (hero == null)
				return "No hero";

			return hero.Name;
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}

		#endregion

	}
}

