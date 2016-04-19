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
		/// Activate the necromancer. First he detects and then moves.
		/// </summary>
		/// <param name="gameState">Game state.</param>
		/// <param name="detectedHero">Force this hero to be detected.</param>
		/// <param name="roll">Force the necromancer roll.</param>
		/// <param name="heroesToIgnore">Heroes that are ignored due to Elusive Spirit or Blinding Black</param>
		public NecromancerActivationResult Activate (GameState gameState, 
		                                             Hero detectedHero = null,
		                                             int? roll = null, 
		                                             int[] heroesToIgnore = null)
		{
			var result = new NecromancerActivationResult ();
			result.NecromancerRoll = roll.HasValue ? roll.Value : _d6GeneratorService.RollDemBones ();
			result.OldLocationId = gameState.Necromancer.LocationId;

			result.DarknessIncrease = 1 + gameState.Locations.SelectMany (x => x.Blights).Count (x => x.Name == "Desecration");
			if (gameState.Heroes.Any (x => x.HasShieldOfRadiance) && result.NecromancerRoll == 6) {
				result.DarknessIncrease--;
				result.Notes += "Shield of Radiance triggered" + Environment.NewLine;
			}

			gameState.Darkness += result.DarknessIncrease;
				
			//detect
			if (detectedHero == null)
				result.DetectedHero = Detect (gameState, result.NecromancerRoll, heroesToIgnore);
			else
				result.DetectedHero = detectedHero;

			//move
			var newLocation = Move (gameState, detectedHero, result.NecromancerRoll);
			result.NewLocationId = newLocation.Id;

			//spawn
			var spawnResult = Spawn(gameState, newLocation, result.NecromancerRoll);
			result.NewBlights = spawnResult.NewBlights;
			result.SpawnQuest = spawnResult.SpawnQuest;

			return result;
		}

		/// <summary>
		/// Returns a hero visible to the necromancer
		/// </summary>
		/// <param name="gameState">Game state.</param>
		/// <param name="roll">The necromancer's roll.</param>
		/// <param name="heroesToIgnore">Heroes to ignore even if they can be detected (from powers like Blinding Black for example)</param>
		public Hero Detect (GameState gameState, int roll, int[] heroesToIgnore)
		{			
			var detectionRoll = gameState.GatesActive ? roll + 1 : roll;
			//first check if any heroes can be detected
			var exposedHeroes = gameState.Heroes.Where (x => x.LocationId != (int)LocationIds.Monastery && x.Secrecy < detectionRoll && (heroesToIgnore == null || !heroesToIgnore.Contains (x.Id)));

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
					return exposedHeroes.ToArray () [index];
				} else if (exposedHeroes.Any (x => x.LocationId == gameState.Necromancer.LocationId)) {
					return exposedHeroes.First (x => x.LocationId == gameState.Necromancer.LocationId);
				} else {
					//TODO: this needs work. not exactly readable at the moment
					var necromancerLocation = gameState.Locations.First (x => x.Id == gameState.Necromancer.LocationId);

					var heroesOneLocationAway = exposedHeroes.Where (x => necromancerLocation.Pathways.Any (y => y == x.LocationId));
					if (heroesOneLocationAway.Any ())
						exposedHeroes = heroesOneLocationAway;

					var index = new Random ().Next (0, exposedHeroes.Count () - 1);
					return exposedHeroes.ToArray () [index];
				}
			} else
				return null;
		}

		/// <summary>
		/// Moves the necromancer in the detected hero's direction or via the die roll if no hero passed in.
		/// Return the Location that the necromancer moved to.
		/// </summary>
		/// <param name="gameState">Game state.</param>
		/// <param name="detectedHero">Detected hero.</param>
		/// <param name="roll">Roll.</param>
		public Location Move (GameState gameState, Hero detectedHero, int roll)
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
				newLocation = gameState.Locations.Single (x => x.Id == currentLocation.Pathways [roll - 1]);

			var conjurer = gameState.Heroes.GetHero<Conjurer> ();
			if (conjurer != null
			   && conjurer.InvisibleBarrierLocationId.HasValue
			   && conjurer.InvisibleBarrierLocationId.Value == newLocation.Id) 
			{
				newLocation = currentLocation;
				conjurer.InvisibleBarrierLocationId = null;
				//result.Notes += string.Format ("Invisible Barrier deactivated and cancelled the Necromancer's movement. {0}", Environment.NewLine);
			}

			gameState.Necromancer.LocationId = newLocation.Id;

			return newLocation;
		}

		public NecromancerSpawnResult Spawn (GameState gameState, Location newLocation, int necromancerRoll)
		{
			var result = new NecromancerSpawnResult() { NewBlights = new List<Tuple<Location, Blight>>()};

			//check if a quest needs to be spawned
			if (gameState.PallOfSuffering && (necromancerRoll == 3 || necromancerRoll == 4))
				result.SpawnQuest = true;

			var initialBlightCount = newLocation.BlightCount;
			var monastery = gameState.Locations.Single (l => l.Id == (int)LocationIds.Monastery);

			//spawn blight at necromancer's new location
			result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));		

			//standard darkness track effects
			if (gameState.DarknessTrackEffectsActive) {
				if (gameState.Darkness >= 10 && newLocation.BlightCount == 0)
					result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));

				if (gameState.Darkness >= 20 && (necromancerRoll == 1 || necromancerRoll == 2))
					result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));
			}

			//darkness card effects
			//if (gameState.Mode != DarknessCardsMode.None) {
				if (gameState.Necromancer.FocusedRituals
					&& !gameState.Heroes.Any (x => x.LocationId == newLocation.Id))
					result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));

				if (gameState.Necromancer.CreepingShadows && (necromancerRoll == 5 || necromancerRoll == 6))
					result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));

				if (gameState.Necromancer.DyingLand) {
					if (gameState.DarknessTrackEffectsActive
						&& gameState.Darkness >= 10
						&& newLocation.BlightCount == 1)
						result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));
					else if ((!gameState.DarknessTrackEffectsActive
						|| gameState.Darkness < 10)
						&& newLocation.BlightCount == 0)
						result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));
				}

				if (gameState.Necromancer.EncroachingShadows && necromancerRoll == 6)
					result.NewBlights.Add (_blightService.SpawnBlight (monastery, gameState));

				if (gameState.Necromancer.Overwhelm && initialBlightCount < 4 && newLocation.BlightCount >= 4)
					result.NewBlights.Add (_blightService.SpawnBlight (monastery, gameState));

			//}

			//darkness tipping over 30
			if (gameState.Darkness > 30) {
				var overflowCount = gameState.Darkness - 30;

				for (int i = 0; i < overflowCount; i++)
					result.NewBlights.Add (_blightService.SpawnBlight (monastery, gameState));

				gameState.Darkness = 30;
			}

			return result;
		}
	}

	public class NecromancerActivationResult
	{
		public Hero DetectedHero { get; set; }

		public int DarknessIncrease {get;set;}

		public int NecromancerRoll { get; set; }

		public int OldLocationId { get; set; }

		public int NewLocationId { get; set; }

		public List<Tuple<Location, Blight>> NewBlights { get; set; }

		public bool SpawnQuest { get; set; }

		public string Notes{ get; set; }
	}

	public class NecromancerSpawnResult
	{
		public List<Tuple<Location, Blight>> NewBlights { get; set; }

		public bool SpawnQuest { get; set; }

		public string Notes { get; set; }
	}
}
