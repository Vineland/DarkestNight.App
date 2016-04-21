using System;
using GalaSoft.MvvmLight;
using Vineland.Necromancer.UI;
using XLabs.Ioc;
using Vineland.Necromancer.Core;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public abstract class BaseViewModel :ViewModelBase
	{
		public BaseViewModel ()
		{
		}

		public NecromancerApp Application
		{
			get { return (Xamarin.Forms.Application.Current as NecromancerApp); }
		}

		bool _isLoading;
		public bool IsLoading{
			get{ return _isLoading; }
			set{
				if (_isLoading != value) {
					_isLoading = value;
					RaisePropertyChanged (() => IsLoading);
				}
			}
		}

		public virtual void OnAppearing() { }

		public virtual void OnDisappearing() { }

		public virtual void OnBackButtonPressed() { }
	}
}

