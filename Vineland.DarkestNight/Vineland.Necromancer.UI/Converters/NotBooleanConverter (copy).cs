using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class IsSelectedConverter : IValueConverter
	{
		public IsSelectedConverter ()
		{
		}

		#region IValueConverter implementation

		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (bool)value ? Color.Red : Color.Transparent;
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

