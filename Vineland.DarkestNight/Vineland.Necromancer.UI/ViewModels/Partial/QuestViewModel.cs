using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class QuestViewModel : ISpawnViewModel
	{
		public ImageSource Image {
			get { return FileImageSource.FromFile("quest"); }
		}

		public bool IsPlaceHolder
		{
			get
			{
				return false;
			}
		}


	}
}

