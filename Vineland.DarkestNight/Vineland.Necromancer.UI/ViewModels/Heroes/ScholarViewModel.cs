using System;
using Vineland.Necromancer.Core;
using System.Linq;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class ScholarViewModel : HeroViewModel
	{
		Scholar _scholar;

		public ScholarViewModel (Scholar scholar) :
			base (scholar)
		{
			_scholar = scholar;
		}

		public Location AncientDefenseLocation{
			get{ 
				if (_scholar.AncientDefenseLocationId.HasValue)
					return Application.CurrentGame.Locations.Single (l => l.Id == _scholar.AncientDefenseLocationId.Value);
				return null;
			}
			set{
				if (value != null)
					_scholar.AncientDefenseLocationId = value.Id;
				else
					_scholar.AncientDefenseLocationId = null;
			}
		}
	}
}

