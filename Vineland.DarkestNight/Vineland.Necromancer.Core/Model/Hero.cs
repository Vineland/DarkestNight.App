using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GalaSoft.MvvmLight;

namespace Vineland.Necromancer.Core
{
	public class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Secrecy { get; set; }
        public int LocationId { get; set; }
		public Expansion Expansion { get; set; }

		public Hero ()
		{
			LocationId = LocationIds.Monastery;
			Secrecy = 6;
		}

		public override string ToString ()
		{
			return Name;
		}
    }
}
