using System;
using Vineland.Necromancer.Core;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class BlightViewModel :BaseViewModel
	{
		public Blight Blight{ get ; private set; }

		public BlightViewModel (Blight blight)
		{
			Blight = blight;
		}

		public bool IsPlaceHolder{
			get { return Blight == null; }
		}

		public ImageSource Image {
			get { 
				if (IsPlaceHolder)
					return ImageSource.FromFile ("plus");

				return ImageSourceUtil.GetBlightImage (Blight.Name); 
			}
		}
	}
}

