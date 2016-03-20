using System;
using System.Collections.Generic;
using System.Linq;
using Vineland.Necromancer.Core;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class MapCardViewModel : BaseViewModel
	{
		MapCard _mapCard;

		public MapCardViewModel (MapCard mapCard)
		{
			_mapCard = mapCard;

			Rows = new List<MapCardRowModel> ();
			for (int i = 0; i < _mapCard.Rows.Count; i++) {
				var row = _mapCard.Rows [i];
				Rows.Add (new MapCardRowModel () {
					LocationName = Application.CurrentGame.Locations.Single (l => l.Id == row.LocationId).Name,
					BlightImage = ImageSourceUtil.GetBlightImage (row.BlightName),
					ItemImage = ImageSourceUtil.GetItemImage (row.ItemName),
					BackgroundColor = (i % 2 == 0 ? Color.FromHex("#66666666"): Color.Transparent)
				});
			}
				
		}

		public List<MapCardRowModel> Rows {get;set;}

		public void Discard()
		{	
			Application.CurrentGame.MapCards.Discard (_mapCard);
		}

		public void ReturnToTop(){
			Application.CurrentGame.MapCards.Return(_mapCard);
		}

		public void ReturnToBottom(){
			Application.CurrentGame.MapCards.Return (_mapCard, DeckPosition.Bottom);
		}


	}

	public class MapCardRowModel
	{
		public string LocationName{get;set;}
		public ImageSource BlightImage {get;set;}
		public ImageSource ItemImage {get;set;}
		public Color BackgroundColor {get;set;}
	}
}

