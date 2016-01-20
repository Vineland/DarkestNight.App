using System;
using Vineland.DarkestNight.Core;

namespace Vineland.Necromancer.UI
{
	public class NecromancerRollViewModel :BaseViewModel
	{
		NecromancerService _necromancerService;
		NavigationService _navigationService;

		public NecromancerRollViewModel (NecromancerService necromancerService, NavigationService navigationService)
		{
			_necromancerService = necromancerService;
			_navigationService = navigationService;
		}
	}
}

