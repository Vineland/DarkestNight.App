using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class StringToImageSourceConverter : IValueConverter
	{

		#region IValueConverter implementation

		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ImageSource.FromFile((value as string).Replace (" ", string.Empty).ToLower ());
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

