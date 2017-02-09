using System;
using System.Collections.Generic;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.Core.Models
{
	public class NecromancerActivationResult
	{
		public Hero DetectedHero { get; set; }

		public int DarknessAdjustment { get; set; }

		public int NecromancerRoll { get; set; }

		public LocationId OldLocationId { get; set; }

		public LocationId NewLocationId { get; set; }

		public List<SpawnBlightResult> NewBlights { get; set; }

		public bool SpawnQuest { get; set; }

		public string Notes { get; set; }
	}
}
