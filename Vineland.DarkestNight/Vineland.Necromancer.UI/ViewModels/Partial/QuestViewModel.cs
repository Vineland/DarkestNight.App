using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class QuestViewModel : SpawnModelBase
	{
		public override ImageSource Image {
			get { return FileImageSource.FromFile("quest"); }
		}
	}
}

