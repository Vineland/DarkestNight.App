using System;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class NecromancerPhaseViewModel : BaseViewModel
	{
		public NecromancerPhaseViewModel (Settings settings)
		{
		}

		public Settings Settings { get; private set; }

		public bool ShowDarknessCardOptions{
			get { return Application.CurrentGame.Mode != DarknessCardsMode.None; }
		}

		public int Darkness{
			get { return Application.CurrentGame.Darkness; }
			set{ Application.CurrentGame.Darkness = value; }
		}

		public List<Location> AllLocations{
			get { return Location.All; }
		}

		public NecomancerState Necromancer{
			get{ return Application.CurrentGame.Necromancer; }
		}

		public RelayCommand Detect{
			get{
				return new RelayCommand (() => {
					Application.SaveCurrentGame();
					Application.Navigation.Push<NecromancerResultsPage>();
				});
			}
		}
	}
}

