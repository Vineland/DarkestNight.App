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
        public LocationDistance[] Distances { get; set; }
    }

    public class LocationDistance
    {
        public int LocationId { get; set; }
        public int Distance { get; set; }
    }


}
