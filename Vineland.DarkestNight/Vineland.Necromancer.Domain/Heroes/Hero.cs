using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Vineland.Necromancer.Domain
{
	public class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Secrecy { get; set; }
		public int SecrecyDefault { get; set; }
		public int Grace { get; set; }
		public int GraceDefault { get; set; }
		public LocationId LocationId { get; set; }
		public Expansion Expansion { get; set; }

		#region Artifacts
		public bool HasVoidArmour { get; set; }
		public bool HasShieldOfRadiance { get; set; }
		#endregion

		public override string ToString ()
		{
			return Name;
		}

		public string ImageName{
			get { return "hero_" + Name.Replace (" ", "").ToLower (); }
		}
    }
}
