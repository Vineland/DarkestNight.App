using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Vineland.Necromancer.Core
{
	public class Location: IComparable
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int[] Pathways { get; set; }

		public List<Blight> Blights { get; set; }

		public Location ()
		{
			Blights = new List<Blight> ();
		}

		public int BlightCount
		{
			get{ 
				if(Blights.Any())
					return Blights.Sum (x => x.Weight); 
				return 0;
			}
		}

		public override string ToString ()
		{
			return this.Name;
		}

		public int CompareTo (object obj)
		{
			var location = obj as Location;
			if (this.Id < location.Id)
				return -1;
			if (this.Id == location.Id)
				return 0;
			
			return 1;
		}
	}

	public enum LocationIds
	{
		Monastery = 0,
		Mountains = 1,
		Ruins = 2,
		Forest = 3,
		Castle = 4,
		Swamp = 5,
		Village = 6
	}


}
