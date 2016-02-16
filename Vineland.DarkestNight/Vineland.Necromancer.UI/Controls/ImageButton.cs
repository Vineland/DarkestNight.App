using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class ImageButton : Image
	{
		public ImageButton ()
		{
		}


		ImageSource _enabledSource;
		bool _isEnabled;
		public new bool IsEnabled {
			get{ return _isEnabled; }
			set { 
				if (_isEnabled != value) {
					_isEnabled = value; 
					if (_isEnabled) {
						Source = _enabledSource;
					} else {
						_enabledSource = Source;
						Source = DisabledSource;
					}
				}
			}
		}

		public ImageSource DisabledSource {get;set;}
	}
}

