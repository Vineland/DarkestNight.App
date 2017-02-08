using System;
using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.Core;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class AddBlightModel: SpawnModelBase
	{
		public override ImageSource Image
		{
			get { return FileImageSource.FromFile("plus"); }
		}

		public override RelayCommand OnTap
		{
			get
			{
				return new RelayCommand(() => { MessagingCenter.Send<AddBlightModel>(this, "AddBlight"); });
			}
		}
	}
}
