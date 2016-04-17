using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Vineland.Necromancer.Core
{
	public class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Secrecy { get; set; }
		public int SecrecyDefault { get; set; }
		public int Grace { get; set; }
		public int GraceDefault { get; set; }
        public int LocationId { get; set; }
		public Expansion Expansion { get; set; }

		#region Artifacts
		public bool HasVoidArmor { get; set; }
		public bool HasShieldOfRadiance { get; set; }
		public bool HasSeeingGlass { get; set; }
		#endregion

		public override string ToString ()
		{
			return Name;
		}

		public string ImageName{
			get { return Name.Replace (" ", "").ToLower (); }
		}
    }
}
