using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Vineland.Necromancer.Core
{
	public class Deck<T>
	{
		public Deck ()
		{
		}

		public void Initialise (List<T> cards)
		{
			DrawPile = cards;
			DiscardPile = new List<T> ();
		}

		public List<T> DrawPile { get; set; }

		public List<T> DiscardPile { get; set; }

		public T Draw ()
		{
			CheckDrawPile ();

			var card = DrawPile.First();

			return card;
		}

		public void Discard (T card)
		{

			if (DrawPile.Contains (card))
				DrawPile.Remove (card);
			
			DiscardPile.Add (card);			
		}

		public void Place (T card, DeckPosition position)
		{
			switch (position) {
			case DeckPosition.Top:
				DrawPile.Insert (0, card);
				break;
			case DeckPosition.Bottom:
				DrawPile.Add (card);
				break;
			}
		}

		public T Take(){
			var card = Draw ();
			DrawPile.Remove (card);

			return card;
		}

		private void CheckDrawPile ()
		{
			if (!DrawPile.Any ()) {
				DiscardPile.Shuffle ();

				DrawPile = DiscardPile;

				DiscardPile = new List<T> ();
			}
		}
	}

	public enum DeckPosition
	{
		Top,
		Bottom
	}
}

