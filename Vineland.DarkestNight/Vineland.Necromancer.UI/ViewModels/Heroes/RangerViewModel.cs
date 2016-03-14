using System;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class RangerViewModel : HeroViewModel
	{
		Ranger _ranger;

		public RangerViewModel (Ranger ranger)
			: base (ranger)
		{
			_ranger = ranger;
		}

		public bool HermitActive {
			get{ return _ranger.HermitActive; }
			set{ _ranger.HermitActive = value; }
		}
	}
}

