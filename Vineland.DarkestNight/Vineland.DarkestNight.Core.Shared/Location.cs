using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vineland.DarkestNight.Core
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfBlights { get; set; }
        public int[] Pathways { get; set; }

		public static List<Location> All = new List<Location>()
		{
			new Location(){
				Id=1,
				Name="Monastery",
				Pathways=new int[0]{}
			},
			new Location(){
				Id=2,
				Name="Mountains",
				Pathways=new int[6]{ 3, 4, 2, 4, 3, 2 }
			},
			new Location(){
				Id= 3,
				Name= "Castle",
				Pathways=new int[6]{ 5, 4, 2, 3, 4, 3 }
			},
			new Location(){
				Id=4,
				Name="Village",
				Pathways=new int[6]{ 2, 6, 7, 3, 5, 4 }
			},
			new Location(){
				Id= 5,
				Name= "Swamp",
				Pathways= new int[6] { 6, 4, 3, 5, 4, 5 }
			},
			new Location(){
				Id= 6,
				Name= "Ruins",
				Pathways=new int[6]{ 7, 5, 4, 6, 4, 6}
			},
			new Location(){
				Id= 7,
				Name="Forest",
				Pathways=new int[]{ 4, 6, 7, 4, 6, 7 }
			}
		};
    }

    public class LocationDistance
    {
        public int LocationId { get; set; }
        public int Distance { get; set; }
    }

    public static class LocationIds
    {
        public static readonly int Monastery = 1;
        public static readonly int Mountains = 2;
        public static readonly int Castle = 3;
        public static readonly int Village = 4;
        public static readonly int Swamp = 5;
        public static readonly int Ruins = 6;
        public static readonly int Forest = 7;
    }


}
