using System;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using Vineland.DarkestNight.UI;
using System.Runtime.InteropServices;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using Android.Util;
using Xamarin.Forms;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Dalvik.SystemInterop;
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

