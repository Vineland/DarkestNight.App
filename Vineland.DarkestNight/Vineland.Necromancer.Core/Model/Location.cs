using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

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

	public static class LocationIds
	{
		public static readonly int Monastery = 0;
		public static readonly int Mountains = 1;
		public static readonly int Castle = 2;
		public static readonly int Village = 3;
		public static readonly int Swamp = 4;
		public static readonly int Ruins = 5;
		public static readonly int Forest = 6;
	}


}
