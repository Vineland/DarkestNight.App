using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Vineland.Necromancer.Core
{
	public class Deck<T>
	{
		public Deck (){
		}

		public void Initialise(List<T> cards)
		{
			DrawPile = cards;
			DiscardPile = new List<T> ();
		}

		public List<T> DrawPile { get; set; }

		public List<T> DiscardPile { get; set; }

		public T DrawCard ()
		{
			CheckDrawPile ();

			var card = DrawPile.First ();

			DrawPile.Remove (card);
			DiscardPile.Add (card);

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
}

