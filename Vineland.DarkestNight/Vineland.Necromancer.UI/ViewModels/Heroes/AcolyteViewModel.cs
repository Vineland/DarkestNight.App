using System;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class AcolyteViewModel : HeroViewModel
	{
		public AcolyteViewModel (Hero hero)
			: base(hero)
		{
		}

		public bool BlindingBlackActive{
			get{ return (_hero as Acolyte).BlindingBlackActive; }
			set{ (_hero as Acolyte).BlindingBlackActive = value; }
		}
	}
}

