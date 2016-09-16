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
		public HeroTurnViewModel (HeroesViewModel heroesViewModel, BlightsViewModel blightsViewModel)
		{
			HeroesSeleted = true;
			HeroesViewModel = heroesViewModel;
			BlightsViewModel = blightsViewModel;

			MessagingCenter.Subscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete", OnNecromancerPhaseComplete);
		}

		public override void Cleanup ()
		{
			base.OnDisappearing ();
			MessagingCenter.Unsubscribe<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete");
		}

		public int Darkness {
			get { return Application.CurrentGame.Darkness; }
			set {
				Application.CurrentGame.Darkness = value;
				RaisePropertyChanged (() => Darkness);
			}
		}

		public HeroesViewModel HeroesViewModel { get; private set; }
		public BlightsViewModel BlightsViewModel { get; private set; }
		public bool HeroesSeleted { get; set; }

		public void OnNecromancerPhaseComplete(NecromancerActivationViewModel sender)
		{
			RaisePropertyChanged (() => Darkness);
		}

		public RelayCommand NextPhase {
			get {
				return new RelayCommand (async () => {
					await Application.Navigation.Push<NecromancerPhasePage> ();
					Application.SaveCurrentGame ();
				});
			}
		}
	}
}



