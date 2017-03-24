using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Vineland.Necromancer.Domain;
using System;

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

		int _darkness;
		public int Darkness 
		{ 
			get { return _darkness; }
			set
			{
				_darkness = value;
				HighestDarkness = Math.Max(HighestDarkness, _darkness);
			}
		}

		public int HighestDarkness { get; set; }

		public int NumberOfPlayers { get; set; }

		public DifficultyLevel DifficultyLevel { get; set; }

		public bool UsingDarknessCards
		{
			get
			{
				return NumberOfDarknessCards > 0;
			}
		}

		public int NumberOfDarknessCards { get; set; }

		public int LastDarknessCardDrawAt { get; set; }
		public int NextDarknessCardDrawAt
		{
			get
			{
				var timings = DarknessCardTimings[NumberOfDarknessCards];
				if (LastDarknessCardDrawAt == 0)
					return timings[0];

				if (LastDarknessCardDrawAt == timings.Last())
					return int.MaxValue;

				return timings.First(x => x > LastDarknessCardDrawAt);
			}
		}

		public bool GatesActive
		{
			get { return Locations.Any(l => l.Blights.Any(b => b.Name == "Gate")); }
		}

		public GameState Clone(){
			var json = JsonConvert.SerializeObject (this, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
			return JsonConvert.DeserializeObject<GameState> (json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
		}

		Dictionary<int, int[]> DarknessCardTimings = new Dictionary<int, int[]>()
		{
			{2, new int []{ 10, 20}},
			{3, new int []{ 7, 14, 21}},
			{4, new int []{ 5, 10, 15, 20}},
			{5, new int []{ 4, 8, 12, 16, 20}},
			{6, new int []{ 2,6,10,14,18,22}},
			{7, new int []{ 0,4,8,12,16,20,24}},
			{8, new int []{ 0,3,6,9,12,15,18,21}}
		};
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
}
