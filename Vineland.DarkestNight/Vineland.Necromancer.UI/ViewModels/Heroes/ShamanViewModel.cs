using System;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
using Android.Util;
using Xamarin.Forms;
using Java.Lang;

namespace Vineland.Necromancer.UI
{
	public class ShamanViewModel : HeroViewModel
	{
		Shaman _shaman;

		public ShamanViewModel (Shaman shaman)
			: base (shaman)
		{
			_shaman = shaman;

			MessagingCenter.Subscribe<MapCardViewModel> (this, "SpiritSightCardReturned", SpiritSightCardReturned);
		}

		public override void Cleanup ()
		{
			base.Cleanup ();
			MessagingCenter.Unsubscribe<MapCardViewModel> (this, "SpiritSightCardReturned");
		}

		public string SpiritSightButtonLabel{
			get{
				if (_shaman.SpiritSightMapCard == null)
					return "Draw";
				else
					return "View";
			}
		}

		public RelayCommand SpiritSightCommand{
			get{
				return new RelayCommand (() => {
					if(_shaman.SpiritSightMapCard == null)
					{
						_shaman.SpiritSightMapCard = Application.CurrentGame.MapCards.Draw();
						Application.SaveCurrentGame();
						RaisePropertyChanged(() => SpiritSightButtonLabel);
					}
					
					Application.Navigation.Push<MapCardPage>(new MapCardViewModel(_shaman.SpiritSightMapCard, MapCardViewModel.MapCardContext.SpiritSight));
				});	
			}
		}

		private void SpiritSightCardReturned(MapCardViewModel sender)
		{
			RaisePropertyChanged(() => SpiritSightButtonLabel);
		}
	}
}

