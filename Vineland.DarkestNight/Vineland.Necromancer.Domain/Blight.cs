using System;
using System.Collections.Generic;

namespace Vineland.Necromancer.Domain
{
	public class Blight
	{
		public string Name { get; set; }

		public int Weight { get; set; }

		public bool GlobalEffect { get; set; }

		public Expansion Expansion { get; set; }
	}
}

