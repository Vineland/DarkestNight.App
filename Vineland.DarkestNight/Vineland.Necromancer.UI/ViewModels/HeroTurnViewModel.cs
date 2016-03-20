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

namespace Vineland.Necromancer.UI
{
	public class HeroTurnViewModel : BaseViewModel
	{
		public HeroTurnViewModel (Hero hero)
		{
			HeroViewModel = HeroViewModel.Create(hero);
			BlightLocationsViewModel = Resolver.Resolve<BlightLocationsViewModel> ();
		}

		public HeroViewModel HeroViewModel { get; set; }
		public BlightLocationsViewModel BlightLocationsViewModel { get; private set; }

		public RelayCommand DoneCommand {
			get {
				return new RelayCommand (() => {
					Application.SaveCurrentGame ();
					Application.Navigation.Pop();
				});
			}
		}
	}
}

