using System;
using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.Core;
using Xamarin.Forms;
using System.Linq;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class BlightViewModel : BaseViewModel, ISpawnViewModel
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

		public RelayCommand DestroyBlightCommand
		{
			get
			{
				return new RelayCommand(async () =>{
					if (this.IsPlaceHolder)
						return;

					if (await Application.Navigation.DisplayConfirmation("Destroy Blight", "Are you sure you wish to remove this blight?", "Yes", "No"))
						MessagingCenter.Send<BlightViewModel>(this, "DestroyBlight");
				});
			}
		}

		public RelayCommand MoveBlightCommand
		{
			get
			{
				return new RelayCommand(async () =>
				{
					if (this.IsPlaceHolder)
						return;

					var newLocationName = await Application.Navigation.DisplayActionSheet("New Location", "Cancel", null, Application.CurrentGame.Locations.Select(x => x.Name).ToArray());
					if (newLocationName != "Cancel")
					{
						MessagingCenter.Send<BlightViewModel, MoveBlightArgs>(this, "MoveBlight",
							new MoveBlightArgs()
							{
								BlightViewModel = this,
								NewLocationName = newLocationName
							});
					}
				});
			}
		}
	}
}

