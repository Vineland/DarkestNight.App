using System;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class TamerViewModel : HeroViewModel
	{
		Tamer _tamer;

		public TamerViewModel (Tamer tamer)
			:base(tamer)
		{
			_tamer = tamer;
		}

		public bool HasTameBoar{
			get{ return _tamer.HasTameBoar; }
			set{ _tamer.HasTameBoar = value; }
		}
	}
}

