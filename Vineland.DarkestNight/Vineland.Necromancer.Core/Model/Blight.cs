using System;
using System.Collections.Generic;

namespace Vineland.Necromancer.Core
{
	public class Blight
	{
		public string Name { get; set; }

		public string ImageName{
			get { return string.Format ("blight_{0}", Name.ToLower ().Replace(" ", "_")); }
		}

		public int Weight { get; set; }

		public bool GlobalEffect { get; set; }

		public Expansion Expansion { get; set; }
	}
}

