using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class SearchViewModel : BaseViewModel
	{
		public enum MapDeckContext
		{
			Search
		}

		public SearchViewModel (int cardsToDraw)
		{
			MapCards = new List<MapCardViewModel> ();
			for (int i = 0; i < cardsToDraw; i++)
				MapCards.Add (new MapCardViewModel (Application.CurrentGame.MapCards.Draw ()));
		}

		public List<MapCardViewModel> MapCards { get;set; }

		public void Discard(){
			MapCards.ForEach (x => x.Discard ());
		}

		public void ReturnToTop(){
			MapCards.ForEach (x => x.ReturnToTop ());
		}

		public void ReturnToBottom(){
			MapCards.ForEach (x => x.ReturnToBottom ());
		}
	}
}

