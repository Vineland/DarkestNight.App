using System;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class ParagonViewModel : HeroViewModel
	{
		Paragon _paragon;

		public ParagonViewModel (Paragon paragon)
			:base(paragon)
		{
			_paragon = paragon;
		}

		public bool AuraOfHumilityActive{
			get{ return _paragon.AuraOfHumilityActive; }
			set{ _paragon.AuraOfHumilityActive = value; }
		}
	}
}

