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
