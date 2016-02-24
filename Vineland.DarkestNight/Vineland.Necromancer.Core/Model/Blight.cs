using System;
using System.Collections.Generic;

namespace Vineland.Necromancer.Core
{
	public class Blight
	{
		public string Name { get; set; }

		public int Weight { get; set; }

		public int Number { get; set; }

		public bool GlobalEffect { get; set; }

		public Expansion Expansion { get; set; }

		public Blight Clone(){
			return (Blight)this.MemberwiseClone();
		}
	}
}

