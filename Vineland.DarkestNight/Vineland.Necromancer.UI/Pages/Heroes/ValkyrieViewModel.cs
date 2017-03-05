using System;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class ValkyrieViewModel : HeroViewModel
	{
		Valkyrie _valkyrie;

		public ValkyrieViewModel (Valkyrie valkyrie)
			:base(valkyrie)
		{
			_valkyrie = valkyrie;
		}

		public bool ElusiveSpiritActive{
			get{ return _valkyrie.ElusiveSpiritActive; }
			set{ _valkyrie.ElusiveSpiritActive = value; }
		}
	}
}

