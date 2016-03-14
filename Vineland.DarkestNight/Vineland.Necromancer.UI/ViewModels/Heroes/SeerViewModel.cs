using System;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class SeerViewModel : HeroViewModel
	{
		Seer _seer;

		public SeerViewModel (Seer seer)
			:base(seer)
		{
			_seer = seer;
		}

		public int ProphecyOfDoomRoll{
			get{ return _seer.ProphecyOfDoomRoll; }
			set{ _seer.ProphecyOfDoomRoll = value; }
		}
	}
}

