using System;
using System.Collections.Generic;
using System.Linq;
using Vineland.Necromancer.Core;
using Xamarin.Forms;
using GalaSoft.MvvmLight.Command;

namespace Vineland.Necromancer.UI
{
	public class MapCardViewModel : BaseViewModel
	{
		public enum MapCardContext
		{
			Search,
			SpiritSight,
			SeeingGlass,
			RuneOfClairvoyance
		}

		public MapCard MapCard { get; private set; }

		public MapCardContext Context { get; private set; }

		public MapCardViewModel (MapCard mapCard, MapCardContext context = MapCardContext.Search)
		{
			MapCard = mapCard;
			Context = context;

			Rows = new List<MapCardRowModel> ();
			for (int i = 0; i < MapCard.Rows.Count; i++) {
				var row = MapCard.Rows [i];
				Rows.Add (new MapCardRowModel () {
					LocationName = Application.CurrentGame.Locations.Single (l => l.Id == row.LocationId).Name,
					BlightImage = ImageSourceUtil.GetBlightImage (row.BlightName),
					ItemImage = ImageSourceUtil.GetItemImage (row.ItemName),
					BackgroundColor = (i % 2 == 0 ? Color.FromHex ("#66666666") : Color.Transparent)
				});
			}	
		}

		public List<MapCardRowModel> Rows { get; private set; }

		public string DoneLabel {
			get {
				switch (Context) {
					case MapCardContext.SpiritSight:
					case MapCardContext.SeeingGlass:
						return "Return To Top";
					case MapCardContext.RuneOfClairvoyance:
						return "Return Card";
				}
				return string.Empty;
			}
		}

		public RelayCommand DoneCommand {
			get {
				return new RelayCommand (async () => {
					switch (Context) {
					case MapCardContext.SpiritSight:
						{							
							Application.CurrentGame.MapCards.Return (MapCard);
							Application.CurrentGame.Heroes.GetHero<Shaman>().SpiritSightMapCard = null;
							Application.SaveCurrentGame();
							MessagingCenter.Send<MapCardViewModel> (this, "SpiritSightCardReturned");
							break;
						}
					case MapCardContext.SeeingGlass:
						break;
					case MapCardContext.RuneOfClairvoyance:{
							var option = await Application.Navigation.DisplayActionSheet("Return Card To", "Cancel", null, "Top", "Bottom");
							Application.CurrentGame.MapCards.Return(MapCard, option == "Top" ? DeckPosition.Top: DeckPosition.Bottom);
							Application.SaveCurrentGame();
							break;
						}
					}

					Application.Navigation.Pop ();
				});
			}
		}

		public void ReturnToBottom ()
		{
			Application.CurrentGame.MapCards.Return (MapCard, DeckPosition.Bottom);
			Application.Navigation.Pop ();
		}
	}

	public class MapCardRowModel
	{
		public string LocationName{ get; set; }

		public ImageSource BlightImage { get; set; }

		public ImageSource ItemImage { get; set; }

		public Color BackgroundColor { get; set; }
	}
}

