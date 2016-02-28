using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Vineland.Necromancer.Core
{
	/// <summary>
	/// http://stackoverflow.com/questions/5383498/shuffle-rearrange-randomly-a-liststring
	/// </summary>
	public static class ListExtensions
	{
		public static void Shuffle<T> (this IList<T> list)
		{
			int n = list.Count;
			Random rnd = new Random ();
			while (n > 1) {
				int k = (rnd.Next (0, n) % n);
				n--;
				T value = list [k];
				list [k] = list [n];
				list [n] = value;
			}
		}

		public static T GetHero<T> (this IList<Hero> heroes) where T: Hero
		{
			return heroes.SingleOrDefault (h => h is T) as T;
		}
	}
}

