﻿using System;
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
				Id=0,
				Name="Monastery",
				Pathways=new int[0]{}
			},
			new Location(){
				Id=1,
				Name="Mountains",
				Pathways=new int[6]{ 3, 4, 2, 4, 3, 2 }
			},
			new Location(){
				Id= 2,
				Name= "Castle",
				Pathways=new int[6]{ 5, 4, 2, 3, 4, 3 }
			},
			new Location(){
				Id=3,
				Name="Village",
				Pathways=new int[6]{ 2, 6, 7, 3, 5, 4 }
			},
			new Location(){
				Id= 4,
				Name= "Swamp",
				Pathways= new int[6] { 6, 4, 3, 5, 4, 5 }
			},
			new Location(){
				Id= 5,
				Name= "Ruins",
				Pathways=new int[6]{ 7, 5, 4, 6, 4, 6}
			},
			new Location(){
				Id= 6,
				Name="Forest",
				Pathways=new int[]{ 4, 6, 7, 4, 6, 7 }
			}
		};

		public override string ToString ()
		{
			return this.Name;
		}
    }

    public class LocationDistance
    {
        public int LocationId { get; set; }
        public int Distance { get; set; }
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
