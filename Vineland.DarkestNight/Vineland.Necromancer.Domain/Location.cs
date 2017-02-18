using System;
using System.Collections.Generic;
using System.Linq;

namespace Vineland.Necromancer.Domain
{
	public class Location: IComparable
	{
		public LocationId Id { get; set; }

		public string Name { get; set; }

		public LocationId[] Pathways { get; set; }

		public List<Blight> Blights { get; set; }

		public List<Quest> Quests { get; set; }

		public Location ()
		{
			Blights = new List<Blight> ();
			Quests = new List<Quest>();
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

	public enum LocationId
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
