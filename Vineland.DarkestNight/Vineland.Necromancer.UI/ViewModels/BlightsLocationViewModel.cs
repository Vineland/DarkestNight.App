using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class BlightLocationsViewModel : BaseViewModel
	{
		GameStateService _gameStateService;

		public BlightLocationsViewModel (GameStateService gameStateService)
		{
			_gameStateService = gameStateService;
		}

		public List<Location> Locations {
			get{ return _gameStateService.CurrentGame.Locations; }
		}

		public override void OnBackButtonPressed ()
		{
			_gameStateService.Save ();
		}
	}
}

