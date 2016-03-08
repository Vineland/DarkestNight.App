using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vineland.Necromancer.Core.Services;
using System.Globalization;
using System.Xml.Linq;
using System.Threading;
using Newtonsoft.Json.Serialization;

namespace Vineland.Necromancer.Core
{
	public class NecromancerService
	{
		D6GeneratorService _d6GeneratorService;
		BlightService _blightService;

		public NecromancerService (D6GeneratorService d6GeneratorService, BlightService blightService)
		{
			_d6GeneratorService = d6GeneratorService;
			_blightService = blightService;
		}

		/// <summary>
		/// Activate the necromancer. First he detects, then moves and finally spawns blights/quests.
		/// </summary>
		/// <param name="gameState">Game state.</param>
		/// <param name="detectedHero">Force this hero to be detected.</param>
		/// <param name="roll">Force the necromancer roll.</param>
		/// <param name="heroesToIgnore">Heroes that are ignored due to Elusive Spirit or Blinding Black</param>
		public NecromancerDetectionResult Activate (GameState gameState, 
		                                             Hero detectedHero = null,
		                                             int? roll = null, 
		                                             int[] heroesToIgnore = null)
		{
			var result = new NecromancerDetectionResult ();
			result.MovementRoll = roll.HasValue ? roll.Value : _d6GeneratorService.RollDemBones ();
			result.DetectionRoll = gameState.GatesActive ? result.MovementRoll + 1 : result.MovementRoll;
			result.OldLocation = gameState.Locations.Single (l => l.Id == gameState.Necromancer.LocationId);

			//detect
			if (detectedHero == null)
				Detect (gameState, result, heroesToIgnore);
			else
				result.DetectedHero = detectedHero;

			//move
			result.NewLocation = Move (detectedHero, result.MovementRoll, gameState);
			
			return result;
		}

		private void Detect (GameState gameState, NecromancerDetectionResult result,
		                     int[] heroesToIgnore)
		{
			//first check if any heroes can be detected
			var exposedHeroes = gameState.Heroes.Where (x => x.LocationId != (int)LocationIds.Monastery && x.Secrecy < result.DetectionRoll && (heroesToIgnore == null || !heroesToIgnore.Contains (x.Id)));

			//special hero effects
			var ranger = gameState.Heroes.SingleOrDefault (x => x is Ranger) as Ranger;
			if (ranger != null && ranger.HermitActive && ranger.LocationId == (int)LocationIds.Swamp)
				exposedHeroes = exposedHeroes.Where (h => h != ranger);

			var paragon = gameState.Heroes.SingleOrDefault (h => h is Paragon) as Paragon;
			if (paragon != null && paragon.AuraOfHumilityActive)
				exposedHeroes = exposedHeroes.Where (x => x.LocationId != paragon.LocationId);

			if (exposedHeroes.Any ()) {
				//figure out which hero the necromancer is pursuing

				if (gameState.GatesActive) { 
					//if gates are active pick a random hero from those detectd
					var index = new Random ().Next (0, exposedHeroes.Count () - 1);
					result.DetectedHero = exposedHeroes.ToArray () [index];
				} else if (exposedHeroes.Any (x => x.LocationId == gameState.Necromancer.LocationId)) {
					result.DetectedHero = exposedHeroes.First (x => x.LocationId == gameState.Necromancer.LocationId);
				} else {
					//TODO: this needs work. not exactly readable at the moment
					var necromancerLocation = gameState.Locations.First (x => x.Id == gameState.Necromancer.LocationId);

					var heroesOneLocationAway = exposedHeroes.Where (x => necromancerLocation.Pathways.Any (y => y == x.LocationId));
					if (heroesOneLocationAway.Any ())
						exposedHeroes = heroesOneLocationAway;

					var index = new Random ().Next (0, exposedHeroes.Count () - 1);
					result.DetectedHero = exposedHeroes.ToArray () [index];
				}
			} else
				result.DetectedHero = null;
		}

		/// <summary>
		/// Moves the necromancer to a new location and returns the location moved to.
		/// </summary>
		/// <param name="detectedHero">Detected hero.</param>
		/// <param name="movementRoll">Movement roll.</param>
		/// <param name="gameState">Game state.</param>
		private Location Move (Hero detectedHero, int movementRoll, GameState gameState)
		{
			var currentLocation = gameState.Locations.Single (x => x.Id == gameState.Necromancer.LocationId);
			Location newLocation = null;

			if (detectedHero != null) {
				var heroLocation = gameState.Locations.Single (x => x.Id == detectedHero.LocationId);
				var necromancerLocation = gameState.Locations.First (x => x.Id == gameState.Necromancer.LocationId);
				//if the necromancer can reach the hero go directly to them
				if (necromancerLocation.Pathways.Contains (heroLocation.Id) || gameState.GatesActive)
					newLocation = heroLocation;
				else {
					//the common locations between the hero and necromancer are the ones that the necro should move along
					var commonLocationIds = necromancerLocation.Pathways.Intersect (heroLocation.Pathways).ToArray ();
					var index = new Random ().Next (0, commonLocationIds.Length - 1);
					newLocation = gameState.Locations.Single (x => x.Id == commonLocationIds [index]);
				}
			} else
				newLocation = gameState.Locations.Single (x => x.Id == currentLocation.Pathways [movementRoll - 1]);

			gameState.Necromancer.LocationId = newLocation.Id;

			return newLocation;
		}

		public NecromancerSpawnResult Spawn (Location location, int necromancerRoll, GameState gameState)
		{
			var notes = new StringBuilder ();
			var result = new NecromancerSpawnResult (){ NewBlights = new List<Tuple<Location, Blight>> () };

			//check if a quest needs to be spawned
			if (gameState.PallOfSuffering && (necromancerRoll == 3 || necromancerRoll == 4))
				result.SpawnQuest = true;

			var initialBlightCount = location.BlightCount;
			var monastery = gameState.Locations.Single (l => l.Id == (int)LocationIds.Monastery);

			//spawn blight at necromancer's new location
			result.NewBlights.Add (_blightService.SpawnBlight (location, gameState));		

			//standard darkness track effects
			if (gameState.DarknessTrackEffectsActive) {
				if (gameState.Darkness >= 10 && location.BlightCount == 0)
					result.NewBlights.Add (_blightService.SpawnBlight (location, gameState));

				if (gameState.Darkness >= 20 && (necromancerRoll == 1 || necromancerRoll == 2))
					result.NewBlights.Add (_blightService.SpawnBlight (location, gameState));
			}

			//darkness card effects
			if (gameState.Mode != DarknessCardsMode.None) {
				if (gameState.Necromancer.FocusedRituals
				    && !gameState.Heroes.Any (x => x.LocationId == location.Id))
					result.NewBlights.Add (_blightService.SpawnBlight (location, gameState));

				if (gameState.Necromancer.CreepingShadows && (necromancerRoll == 5 || necromancerRoll == 6))
					result.NewBlights.Add (_blightService.SpawnBlight (location, gameState));

				if (gameState.Necromancer.DyingLand) {
					if (gameState.DarknessTrackEffectsActive
					    && gameState.Darkness >= 10
					    && location.BlightCount == 1)
						result.NewBlights.Add (_blightService.SpawnBlight (location, gameState));
					else if ((!gameState.DarknessTrackEffectsActive
					         || gameState.Darkness < 10)
					         && location.BlightCount == 0)
						result.NewBlights.Add (_blightService.SpawnBlight (location, gameState));
				}

				if (gameState.Necromancer.EncroachingShadows && necromancerRoll == 6)
					result.NewBlights.Add (_blightService.SpawnBlight (monastery, gameState));

				if (gameState.Necromancer.Overwhelm && initialBlightCount < 4 && location.BlightCount >= 4)
					result.NewBlights.Add (_blightService.SpawnBlight (monastery, gameState));

			}
			return result;
		}
	}

	public class NecromancerDetectionResult
	{
		public Hero DetectedHero { get; set; }

		public int MovementRoll { get; set; }

		public int DetectionRoll { get; set; }

		public Location OldLocation { get; set; }

		public Location NewLocation { get; set; }

		public string Notes{ get; set; }
	}

	public class NecromancerSpawnResult
	{
		public List<Tuple<Location, Blight>> NewBlights { get; set; }

		public bool SpawnQuest { get; set; }

		public string Notes { get; set; }
	}
}
