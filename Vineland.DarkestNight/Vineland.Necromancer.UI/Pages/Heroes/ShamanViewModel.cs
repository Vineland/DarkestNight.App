﻿using Xamarin.Forms;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class ShamanViewModel : HeroViewModel
	{
		Shaman _shaman;

		public ShamanViewModel (Shaman shaman)
			: base (shaman)
		{
			_shaman = shaman;

			//MessagingCenter.Subscribe<MapCardViewModel> (this, "SpiritSightCardReturned", SpiritSightCardReturned);
		}

		//public override void Cleanup ()
		//{
		//	base.Cleanup ();
		//	MessagingCenter.Unsubscribe<MapCardViewModel> (this, "SpiritSightCardReturned");
		//}

		public string SpiritSightButtonLabel{
			get{
				if (_shaman.SpiritSightMapCard == null)
					return "Draw";
				else
					return "View";
			}
		}

		public Command SpiritSightCommand{
			get{
				return new Command (() => {
					if(_shaman.SpiritSightMapCard == null)
					{
						_shaman.SpiritSightMapCard = Application.CurrentGame.MapCards.Draw();
						Application.SaveCurrentGame();
						RaisePropertyChanged("SpiritSightButtonLabel");
					}
					
					//Application.Navigation.Push<MapCardPage>(new MapCardViewModel(_shaman.SpiritSightMapCard, MapCardViewModel.MapCardContext.SpiritSight));
				});	
			}
		}

		//private void SpiritSightCardReturned(MapCardViewModel sender)
		//{
		//	RaisePropertyChanged("SpiritSightButtonLabel");
		//}
	}
}

