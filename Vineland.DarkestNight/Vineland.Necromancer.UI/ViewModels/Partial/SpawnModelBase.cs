using System;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public abstract class SpawnModelBase
	{
		public abstract ImageSource Image { get; }

		public virtual RelayCommand OnTap { get; }

		public virtual RelayCommand OnLongPress { get; }
	}
}

