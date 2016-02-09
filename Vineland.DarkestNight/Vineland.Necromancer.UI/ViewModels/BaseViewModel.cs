using System;
using GalaSoft.MvvmLight;
using Vineland.Necromancer.UI;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public abstract class BaseViewModel :ViewModelBase
	{
		public BaseViewModel ()
		{
		}

		public NavigationService Navigation {
			get { return Resolver.Resolve<NavigationService> (); }
		}

		public virtual void OnAppearing() { }

		public virtual void OnDisappearing() { }

		public virtual void OnBackButtonPressed() { }
	}
}

