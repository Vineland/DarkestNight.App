﻿using System;
using Vineland.DarkestNight.Core;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class NecromancerPhaseViewModel : BaseViewModel
	{
		NavigationService _navigationService;
		SaveGameService _saveGameService;

		public NecromancerPhaseViewModel (NavigationService navigationService, SaveGameService saveGameService)
		{
			_navigationService = navigationService;
			_saveGameService = saveGameService;
		}

		public int DarknessLevel{
			get { return App.CurrentGame.DarknessLevel; }
		}

		public List<Location> AllLocations{
			get { return Location.All; }
		}

		public NecomancerState Necromancer{
			get{ return App.CurrentGame.Necromancer; }
		}

		public RelayCommand DetectionRollCommand{
			get{
				return new RelayCommand (() => {
					_saveGameService.Save(App.CurrentGame);
					_navigationService.PushViewModel<NecromancerRollViewModel>();
				});
			}
		}


	}
}
