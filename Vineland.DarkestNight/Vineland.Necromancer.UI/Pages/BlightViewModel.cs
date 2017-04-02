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

		bool _isSelected = false;
		public bool IsSelected
		{get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				RaisePropertyChanged("IsSelected");
			}
		}

		public Command DestroyBlightCommand
		{
			get
			{
				return new Command(async () =>{
					if (this.IsPlaceHolder)
						return;

					if (await CoreMethods.DisplayAlert("Destroy Blight", "Are you sure you wish to remove this blight?", "Yes", "No"))
						MessagingCenter.Send<BlightViewModel>(this, "DestroyBlight");
				});
			}
		}

		public Command MoveBlightCommand
		{
			get
			{
				return new Command(async (obj) =>
				{
					if (this.IsPlaceHolder)
						return;

					var newLocationName = await CoreMethods.DisplayActionSheet("New Location", "Cancel", null, Application.CurrentGame.Locations.Select(x => x.Name).ToArray());
					if (newLocationName != "Cancel")
					{
						//MessagingCenter.Send<BlightViewModel, MoveBlightArgs>(this, "MoveBlight",
						//	new MoveBlightArgs()
						//	{
						//		BlightViewModel = this,
						//		NewLocationName = newLocationName
						//	});
					}
				});
			}
		}
	}
}

