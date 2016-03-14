using System;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class WayfarerViewModel :HeroViewModel
	{
		Wayfarer _wayfarer;

		public WayfarerViewModel (Wayfarer wayfarer)
			:base(wayfarer)
		{
			_wayfarer = wayfarer;
		}

		public bool DecoyActive{
			get{ return _wayfarer.DecoyActive; }
			set{ _wayfarer.DecoyActive = value; }
		}
	}
}

