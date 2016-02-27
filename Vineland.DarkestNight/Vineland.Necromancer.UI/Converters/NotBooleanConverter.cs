using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class NotBooleanConverter : IValueConverter
	{
		public NotBooleanConverter ()
		{
		}

		#region IValueConverter implementation

		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return !(bool)value;
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return !(bool)value;
		}

		#endregion
	}
}

