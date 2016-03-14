using System;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class ShamanViewModel : HeroViewModel
	{
		Shaman _shaman;

		public ShamanViewModel (Shaman shaman)
			: base (shaman)
		{
			_shaman = shaman;
		}

		public MapCard SpiritSightCard{
			get{ return _shaman.SpiritSightMapCard; }
			set{ _shaman.SpiritSightMapCard = value; }
		}

		public string SpiritSightButtonLabel{
			get{
				if (SpiritSightCard == null)
					return "Draw Card";
				else
					return "View Card";
			}
		}
	}
}

