using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.Core
{
	public class GameState
	{
		public GameState ()
		{
			Necromancer = new NecomancerState ();
			Heroes = new List<Hero> ();
		}

		public List<Location> Locations { get; set; }

		public Deck<MapCard> MapCards { get; set; }

		public List<Blight> BlightPool { get; set; }

		public List<Hero> Heroes { get; protected set; }

		public NecomancerState Necromancer { get; protected set; }

		public int Darkness { get; set; }

		public int NumberOfPlayers {get;set;}

		public DifficultyLevel DifficultyLevel {get;set;}

		public bool GatesActive{
			get{ return Locations.Any (l => l.Blights.Any (b => b.Name == "Gate")); }
		}

		public GameState Clone(){
			var json = JsonConvert.SerializeObject (this, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
			return JsonConvert.DeserializeObject<GameState> (json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
		}
	}

	public class NecomancerState
	{
		public LocationId LocationId { get; set; }

		#region Darkness Cards

		public bool FocusedRituals { get; set; }

		public bool EncroachingShadows { get; set; }

		public bool DyingLand { get; set; }

		public bool Overwhelm { get; set; }

		public bool CreepingShadows { get; set; }

		#endregion
	}

	//public enum DarknessCardsMode
	//{
	//	None,
	//	Standard,
	//	Twilight,
	//	Midnight
	//}
}
