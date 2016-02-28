using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Vineland.Necromancer.Core
{
	public class DataService
	{
		List<Blight> _blights;
		private List<Blight> Blights 
		{
			get{
				if (_blights == null) {
					_blights = JsonConvert.DeserializeObject<List<Blight>> (ReadEmbeddedResource ("Vineland.Necromancer.Core.blights.json"));;
				}
				return _blights;
			}
		}

		List<MapCard> _mapCards;
		private List<MapCard> MapCards 
		{
			get{
				if (_mapCards == null) {
					_mapCards = JsonConvert.DeserializeObject<List<MapCard>> (ReadEmbeddedResource ("Vineland.Necromancer.Core.map-cards.json"));;
				}
				return _mapCards;
			}
		}

		List<Location> _locations;
		private List<Location> Locations 
		{
			get{
				if (_locations == null) {
					_locations = JsonConvert.DeserializeObject<List<Location>> (ReadEmbeddedResource ("Vineland.Necromancer.Core.Resources.locations.json"));;
				}
				return _locations;
			}
		}

		List<Hero> _heroes;
		private List<Hero> Heroes 
		{
			get{
				if (_heroes == null) {
					_heroes = JsonConvert.DeserializeObject<List<Hero>> (ReadEmbeddedResource ("Vineland.Necromancer.Core.Resources.heroes.json"),
						new JsonSerializerSettings 
						{ 
							TypeNameHandling = TypeNameHandling.Objects 
						});;
				}
				return _heroes;
			}
		}

		Settings _settings;

		public DataService (Settings settings)
		{
			_settings = settings;
		}

		public List<Blight> GetBlights ()
		{
			var blights = Blights.Where (x => x.Expansion == Expansion.BaseGame).ToList ();

			if (_settings.OnShiftingWinds)
				blights.AddRange (Blights.Where (x => x.Expansion == Expansion.OnShiftingWinds));

			if (_settings.InTalesOfOld)
				blights.AddRange (Blights.Where (x => x.Expansion == Expansion.InTalesOfOld));

			return blights.OrderBy (x => x.Name).ToList ();
		}

		public List<MapCard> GetMapCards ()
		{
			var mapCards = MapCards.Where (x => x.Expansion == Expansion.BaseGame).ToList ();

			if (_settings.OnShiftingWinds)
				mapCards.AddRange (MapCards.Where (x => x.Expansion == Expansion.OnShiftingWinds));

			if (_settings.InTalesOfOld)
				mapCards.AddRange (MapCards.Where (x => x.Expansion == Expansion.InTalesOfOld));
			
			mapCards.Shuffle ();

			return mapCards;
		}

		public List<Hero> GetAllHeroes(){
			var heroes = Heroes.Where (x => x.Expansion == Expansion.BaseGame).ToList();

			if (_settings.WithAnInnerLight)
				heroes.AddRange (Heroes.Where (x => x.Expansion == Expansion.WithAnInnerLight));

			if (_settings.OnShiftingWinds)
				heroes.AddRange (Heroes.Where (x => x.Expansion == Expansion.OnShiftingWinds));

			if (_settings.FromTheAbyss)
				heroes.AddRange (Heroes.Where (x => x.Expansion == Expansion.FromTheAbyss));

			if (_settings.InTalesOfOld)
				heroes.AddRange (Heroes.Where (x => x.Expansion == Expansion.InTalesOfOld));

			if (_settings.NymphPromo)
				heroes.AddRange (Heroes.Where (x => x.Expansion == Expansion.NymphPromo));

			if (_settings.EnchanterPromo)
				heroes.AddRange (Heroes.Where (x => x.Expansion == Expansion.EnchanterPromo));

			if (_settings.MercenaryPromo)
				heroes.AddRange (Heroes.Where (x => x.Expansion == Expansion.MercenaryPromo));

			if (_settings.TinkerPromo)
				heroes.AddRange (Heroes.Where (x => x.Expansion == Expansion.TinkerPromo));

			return heroes.OrderBy(x=> x.Name).ToList();
		}


		public List<Location> GetLocations(){
			return Locations;
		}

		private string ReadEmbeddedResource (string resourceId)
		{
			var assembly = typeof(DataService).GetTypeInfo ().Assembly;

			using (var reader = new System.IO.StreamReader (assembly.GetManifestResourceStream (resourceId))) {
				return reader.ReadToEnd ();
			}
		}
	}
}

