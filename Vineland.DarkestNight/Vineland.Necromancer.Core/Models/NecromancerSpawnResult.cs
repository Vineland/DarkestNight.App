using System;
using System.Collections.Generic;

namespace Vineland.Necromancer.Core.Models
{
	public class NecromancerSpawnResult
	{
		public List<SpawnBlightResult> NewBlights { get; set; }

		public bool SpawnQuest { get; set; }

		public string Notes { get; set; }
	}
}
