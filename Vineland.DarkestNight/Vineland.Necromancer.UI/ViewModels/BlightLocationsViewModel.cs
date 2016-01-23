using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class BlightLocationsViewModel : BaseViewModel
	{
		public BlightLocationsViewModel ()
		{
		}

		public List<Location> Locations{
			get { return App.CurrentGame.Locations; }
		}
	}
}

