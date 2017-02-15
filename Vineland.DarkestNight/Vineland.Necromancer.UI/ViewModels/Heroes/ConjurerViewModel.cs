using System;
using Vineland.Necromancer.Core;
using System.Linq;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class ConjurerViewModel: HeroViewModel
	{
		Conjurer _conjurer;

		public ConjurerViewModel (Conjurer conjurer)
			:base(conjurer)
		{
			_conjurer = conjurer;
		}


		public Location InvisibleBarrierLocation{
			get{ 
				if (_conjurer.InvisibleBarrierLocationId.HasValue)
					return Application.CurrentGame.Locations.Single (l => l.Id == _conjurer.InvisibleBarrierLocationId.Value);
				return null;
			}
			set{
				if (value != null)
					_conjurer.InvisibleBarrierLocationId = value.Id;
				else
					_conjurer.InvisibleBarrierLocationId = null;
			}
		}
	}
}

