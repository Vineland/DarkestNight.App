using System;
using System.Dynamic;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.Core
{
	public class MapCard
	{
		public int Id { get; set; }

		public Expansion Expansion { get; set; }

		public List<MapCardRow> Rows { get; set; }
	}

	public class MapCardRow
	{
		public int LocationId { get; set; }

		public string BlightName { get; set; }

		public string ItemName { get; set; }
	}
}

