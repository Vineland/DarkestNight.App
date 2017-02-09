using System;

namespace Vineland.Necromancer.Domain
{
	public class Conjurer : Hero
	{
		public LocationId? InvisibleBarrierLocationId 
		{
			get;
			set;
		}
	}
}