using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GalaSoft.MvvmLight;

namespace Vineland.Necromancer.Core
{
	public class Hero : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Secrecy { get; set; }
        public int LocationId { get; set; }

		public bool IsActive { get; set; }
		public bool HasFallen { get; set; }

		public Hero ()
		{
			LocationId = LocationIds.Monastery;
			Secrecy = 6;
		}

		public static List<Hero> All= new List<Hero>(){
			new Hero() { Id=1, Name="Alchemist"},
			new Hero() { Id=2, Name="Acolyte", Secrecy=7},
			new Hero() { Id=3, Name="Bard", Secrecy = 4 },
			new Hero() { Id=4, Name="Channeler"},
			new Hero() { Id=5, Name="Conjurer", Secrecy=5},
			new Hero() { Id=6, Name="Crusader", Secrecy=4},
			new Hero() { Id=7, Name="Druid"},
			new Hero() { Id=8, Name="Enchanter", Secrecy=5},
			new Hero() { Id=9, Name="Excorcist"},
			new Hero() { Id=10, Name="Knight"},
			new Hero() { Id=11, Name="Mercenary"},
			new Hero() { Id=12, Name="Mesmer"},
			new Hero() { Id=13, Name="Monk"},
			new Hero() { Id=14, Name="Nymph"},
			new Hero() { Id=15, Name="Paragon", Secrecy=4},
			new Hero() { Id=16, Name="Priest"},
			new Hero() { Id=17, Name="Prince", Secrecy=4},
			new Hero() { Id=18, Name="Ranger", Secrecy=7},
			new Hero() { Id=19, Name="Rogue", Secrecy=7},
			new Hero() { Id=20, Name="Scholar"},
			new Hero(){ Id=21, Name="Scout", Secrecy=7},
			new Hero() { Id=22, Name="Seer"},
			new Hero() { Id=23, Name="Shaman", Secrecy=7},
			new Hero() { Id=24, Name="Tamer", Secrecy=5},
			new Hero() { Id=25, Name="Valkyrie", Secrecy=5},
			new Hero() { Id=26, Name="Wayfarer", Secrecy=5},
			new Hero() { Id=27, Name="Wind Dancer"},
			new Hero() { Id=28,Name="Wizard", Secrecy=5},
			new Hero() { Id=29, Name="Tinker"}
		};

		public override string ToString ()
		{
			return Name;
		}
    }
}
