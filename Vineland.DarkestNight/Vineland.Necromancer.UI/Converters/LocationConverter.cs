using System;
using Xamarin.Forms;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class LocationConverter : IValueConverter
	{
		#region IValueConverter implementation

		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var location = value as Location;
			if (location == null)
				return "No location";

			return location.Name;
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}

		#endregion

	}
}

