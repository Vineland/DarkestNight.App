using System;
using Vineland.Necromancer.Core;

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

