using System;
using GalaSoft.MvvmLight;

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
	}
}

