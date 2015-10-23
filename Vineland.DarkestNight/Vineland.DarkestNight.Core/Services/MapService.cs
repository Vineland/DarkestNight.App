using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vineland.DarkestNight.Core.Model;

namespace Vineland.DarkestNight.Core.Services
{
    public class MapService
    {
        public List<Location> Locations { get; private set; }

        public MapService()
        {
            using(var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Vineland.DarkestNight.Core.locations.json"))
            using (var reader = new StreamReader(stream))
            {
                Locations = JsonConvert.DeserializeObject<List<Location>>(reader.ReadToEnd());
            }
        }
    }

    
}
