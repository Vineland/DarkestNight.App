using System;
using GalaSoft.MvvmLight;
using Vineland.Necromancer.UI;

namespace Vineland.Necromancer.UI
{
	public abstract class BaseViewModel :ViewModelBase
	{
		public BaseViewModel ()
		{
		}

		public NecromancerApp App{
			get{
				return NecromancerApp.Current as NecromancerApp;
			}
		}

//		public NavigationService Navigation{
//			get { return Resolver.Resolve<NavigationService> (); }
//		}
	}
}

