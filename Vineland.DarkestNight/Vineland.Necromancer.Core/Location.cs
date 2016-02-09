using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Vineland.Necromancer.Core
{
	public class Location :ObservableObject
	{
		public int Id { get; set; }

		public string Name { get; set; }

		private int _numberOfBlights;

		public int NumberOfBlights { 
			get{ return _numberOfBlights; }
			set {
				if (_numberOfBlights != value)
				{
					_numberOfBlights = value;
					RaisePropertyChanged (() => NumberOfBlights);
				}

			}
		}

		public int[] Pathways { get; set; }

		public static List<Location> All {
			get{ return _all; }
		}

		private static readonly List<Location> _all = new List<Location> (new[] {
			new Location () {
				Id = LocationIds.Monastery,
				Name = "Monastery",
				Pathways = new int[0]{ }
			},
			new Location () {
				Id = LocationIds.Mountains,
				Name = "Mountains",
				Pathways = new int[6]{ 2, 3, 1, 3, 2, 1 }
			},
			new Location () {
				Id = LocationIds.Castle,
				Name = "Castle",
				Pathways = new int[6]{ 4, 3, 1, 2, 3, 2 }
			},
			new Location () {
				Id = LocationIds.Village,
				Name = "Village",
				Pathways = new int[6]{ 1, 5, 6, 2, 4, 3 }
			},
			new Location () {
				Id = LocationIds.Swamp,
				Name = "Swamp",
				Pathways = new int[6] { 5, 3, 2, 4, 3, 4 }
			},
			new Location () {
				Id = LocationIds.Ruins,
				Name = "Ruins",
				Pathways = new int[6]{ 6, 4, 3, 5, 3, 5 }
			},
			new Location () {
				Id = LocationIds.Forest,
				Name = "Forest",
				Pathways = new int[]{ 3, 5, 6, 3, 5, 6 }
			}
		}
		                                              );

		public override string ToString ()
		{
			return this.Name;
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
