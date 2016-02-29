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

		protected List<T> DrawPile { get; set; }

		protected List<T> DiscardPile { get; set; }

		public T Draw ()
		{
			CheckDrawPile ();

			var card = DrawPile.First();

			DrawPile.Remove (card);

			return card;
		}

		public void Discard (T card)
		{

			if (DrawPile.Contains (card))
				DrawPile.Remove (card);
			
			DiscardPile.Add (card);			
		}

		public void Return (T card, DeckPosition position = DeckPosition.Top)
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

