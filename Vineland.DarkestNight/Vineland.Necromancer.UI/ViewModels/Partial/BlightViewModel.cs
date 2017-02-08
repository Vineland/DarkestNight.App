using System;
using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.Core;
using Xamarin.Forms;
using System.Linq;

namespace Vineland.Necromancer.UI
{
	public class BlightViewModel : SpawnModelBase
	{
		public Blight Blight{ get ; private set; }

		public BlightViewModel (Blight blight)
		{
			Blight = blight;
		}

		public override ImageSource Image {
			get { 
				return ImageSourceUtil.GetBlightImage (Blight.Name); 
			}
		}

		public override RelayCommand OnLongPress
		{
			get
			{
				return new RelayCommand(() =>{ MessagingCenter.Send<BlightViewModel>(this, "DestroyBlight"); });
			}
		}

		public override RelayCommand OnTap
		{
			get
			{
				return new RelayCommand(() =>
				{
					MessagingCenter.Send<BlightViewModel>(this, "MoveBlight");
				});
			}
		}
	}
}

