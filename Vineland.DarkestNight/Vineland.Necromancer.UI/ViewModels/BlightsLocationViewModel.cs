using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class BlightLocationsViewModel : BaseViewModel
	{
		public BlightLocationsViewModel ()
		{
		}

		public List<Location> Locations {
			get{ return Application.CurrentGame.Locations; }
		}

		public override void OnBackButtonPressed ()
		{
			Application.SaveCurrentGame();
		}
	}
}

