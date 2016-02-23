using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using System.Linq;

namespace Vineland.Necromancer.Core
{
	public class HeroService
	{
		private static List<Hero> All = new List<Hero> () {
			new Hero () { Id = 1, Name = "Alchemist", Expansion = Expansion.InTalesOfOld },
			new Hero () { Id = 2, Name = "Acolyte", Secrecy = 7, Expansion = Expansion.BaseGame},
			new Hero () { Id = 3, Name = "Bard", Secrecy = 4, Expansion = Expansion.InTalesOfOld},
			new Hero () { Id = 4, Name = "Channeler", Expansion =  Expansion.OnShiftingWinds},
			new Hero () { Id = 5, Name = "Conjurer", Secrecy = 5, Expansion = Expansion.InTalesOfOld },
			new Hero () { Id = 6, Name = "Crusader", Secrecy = 4, Expansion = Expansion.WithAnInnerLight },
			new Hero () { Id = 7, Name = "Druid", Expansion = Expansion.BaseGame},
			new Hero () { Id = 8, Name = "Enchanter", Secrecy = 5, Expansion = Expansion.EnchanterPromo },
			new Hero () { Id = 9, Name = "Exorcist", Expansion = Expansion.FromTheAbyss },
			new Hero () { Id = 10, Name = "Knight", Expansion = Expansion.BaseGame },
			new Hero () { Id = 11, Name = "Mercenary", Expansion = Expansion.MercenaryPromo},
			new Hero () { Id = 12, Name = "Mesmer", Expansion = Expansion.FromTheAbyss},
			new Hero () { Id = 13, Name = "Monk", Expansion = Expansion.WithAnInnerLight },
			new Hero () { Id = 14, Name = "Nymph", Expansion = Expansion.NymphPromo },
			new Hero () { Id = 15, Name = "Paragon", Secrecy = 4, Expansion = Expansion.WithAnInnerLight },
			new Hero () { Id = 16, Name = "Priest", Expansion = Expansion.BaseGame },
			new Hero () { Id = 17, Name = "Prince", Secrecy = 4, Expansion = Expansion.BaseGame },
			new Hero () { Id = 18, Name = "Ranger", Secrecy = 7, Expansion = Expansion.OnShiftingWinds },
			new Hero () { Id = 19, Name = "Rogue", Secrecy = 7, Expansion = Expansion.BaseGame },
			new Hero () { Id = 20, Name = "Scholar", Expansion = Expansion.BaseGame },
			new Hero (){ Id = 21, Name = "Scout", Secrecy = 7, Expansion = Expansion.OnShiftingWinds },
			new Hero () { Id = 22, Name = "Seer", Expansion = Expansion.BaseGame },
			new Hero () { Id = 23, Name = "Shaman", Secrecy = 7, Expansion = Expansion.WithAnInnerLight },
			new Hero () { Id = 24, Name = "Tamer", Secrecy = 5, Expansion = Expansion.InTalesOfOld },
			new Hero () { Id = 29, Name = "Tinker", Expansion = Expansion.TinkerPromo },
			new Hero () { Id = 25, Name = "Valkyrie", Secrecy = 5, Expansion = Expansion.FromTheAbyss },
			new Hero () { Id = 26, Name = "Wayfarer", Secrecy = 5, Expansion = Expansion.OnShiftingWinds },
			new Hero () { Id = 27, Name = "Wind Dancer", Expansion = Expansion.OnShiftingWinds },
			new Hero () { Id = 28, Name = "Wizard", Secrecy = 5, Expansion = Expansion.BaseGame },
		};
		Settings _settings;

		public HeroService (Settings settings)
		{
			_settings = settings;
		}

		public List<Hero> GetAll(){
			var heroes = All.Where (x => x.Expansion == Expansion.BaseGame).ToList();

			if (_settings.WithAnInnerLight)
				heroes.AddRange (All.Where (x => x.Expansion == Expansion.WithAnInnerLight));

			if (_settings.OnShiftingWinds)
				heroes.AddRange (All.Where (x => x.Expansion == Expansion.OnShiftingWinds));

			if (_settings.FromTheAbyss)
				heroes.AddRange (All.Where (x => x.Expansion == Expansion.FromTheAbyss));

			if (_settings.InTalesOfOld)
				heroes.AddRange (All.Where (x => x.Expansion == Expansion.InTalesOfOld));

			if (_settings.NymphPromo)
				heroes.AddRange (All.Where (x => x.Expansion == Expansion.NymphPromo));

			if (_settings.EnchanterPromo)
				heroes.AddRange (All.Where (x => x.Expansion == Expansion.EnchanterPromo));

			if (_settings.MercenaryPromo)
				heroes.AddRange (All.Where (x => x.Expansion == Expansion.MercenaryPromo));

			if (_settings.TinkerPromo)
				heroes.AddRange (All.Where (x => x.Expansion == Expansion.TinkerPromo));

			return heroes.OrderBy(x=> x.Name).ToList();
		}
	}
}

