using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class QuestViewModel : BaseViewModel
	{
		public ImageSource Image {
			get { return FileImageSource.FromFile("quest"); }
		}
	}
}

