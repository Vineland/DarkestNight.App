using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public abstract class SpawnModelBase
	{
		public abstract ImageSource Image { get; }

		public RelayCommand OnLongPress { get; }


	}
}

