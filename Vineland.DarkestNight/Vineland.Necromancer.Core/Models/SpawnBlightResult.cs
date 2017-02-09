using System;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.Core.Models
{
	public class SpawnBlightResult
	{
		public Location Location { get; set; }
		public Blight Blight { get; set; }
	}
}
