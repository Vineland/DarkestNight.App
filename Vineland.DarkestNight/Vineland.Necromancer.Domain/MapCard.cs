using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Vineland.Necromancer.Domain
{
	public class MapCard
	{
		public int Id { get; set; }

		public Expansion Expansion { get; set; }

		public List<MapCardRow> Rows { get; set; }
	}
}

