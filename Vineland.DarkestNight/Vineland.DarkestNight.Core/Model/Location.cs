using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vineland.DarkestNight.Core.Model
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] Pathways { get; set; }
    }
}
