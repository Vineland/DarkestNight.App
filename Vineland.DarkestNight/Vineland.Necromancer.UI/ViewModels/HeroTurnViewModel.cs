using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Vineland.DarkestNight.UI;
using Vineland.Necromancer.Core;
using Xamarin.Forms;
using XLabs.Ioc;
using System.Runtime.CompilerServices;

namespace Vineland.Necromancer.UI
{
	public class HeroTurnViewModel : BaseViewModel
	{
		public HeroTurnViewModel (ActiveHeroesViewModel activeHeroesViewModel, BlightLocationsViewModel blightLocationsViewModel)
		{
			ActiveHeroesViewModel = activeHeroesViewModel;
			BlightLocationsViewModel = blightLocationsViewModel;

			MessagingCenter.Subscribe<NecromancerSpawnViewModel>(this, "NecromancerPhaseComplete", OnNecromancerPhaseComplete);
		}

		public override void Cleanup ()
		{
			MessagingCenter.Unsubscribe<NecromancerSpawnViewModel>(this, "NecromancerPhaseComplete");
		}

		public void OnNecromancerPhaseComplete(NecromancerSpawnViewModel sender){
			
		}

		public ActiveHeroesViewModel ActiveHeroesViewModel { get; private set; }
		public BlightLocationsViewModel BlightLocationsViewModel { get; private set; }


		public RelayCommand NextPhase {
			get {
				return new RelayCommand (() => {
					Application.SaveCurrentGame ();
					Application.Navigation.Push<NecromancerPhasePage> ();
				});
			}
		}
//		public RelayCommand DoneCommand {
//			get {
//				return new RelayCommand (() => {
//					Application.SaveCurrentGame ();
//					Application.Navigation.Pop();
//				});
//			}
//		}
	}
}

