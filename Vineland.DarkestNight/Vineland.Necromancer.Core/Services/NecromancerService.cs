using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vineland.Necromancer.Core.Services;

namespace Vineland.Necromancer.Core
{
    public class NecromancerService
    {
        D6GeneratorService _d6GeneratorService;

        public NecromancerService(D6GeneratorService d6GeneratorService)
        {
            _d6GeneratorService = d6GeneratorService;
        }

		/// <summary>
		/// Activate the necromancer. First he detects, then moves and finally spawns blights/quests.
		/// </summary>
		/// <param name="gameState">Game state.</param>
		/// <param name="detectedHero">Force this hero to be detected.</param>
		/// <param name="roll">Force the detection roll.</param>
		/// <param name="heroesToIgnore">Heroes that are ignored due to Elusive Spirit or Blinding Black</param>
		public NecromancerActivationResult Activate(GameState gameState, 
			Hero detectedHero = null,
			int? roll = null, 
			int[] heroesToIgnore = null){

			var result = new NecromancerActivationResult ();
			result.MovementRoll = roll.HasValue ? roll.Value : _d6GeneratorService.RollDemBones();
			result.DetectionRoll = result.MovementRoll;

			if (gameState.Necromancer.GatesActive)
				result.DetectionRoll++;	

			//detect
			if (detectedHero == null)
				Detect (gameState, result, heroesToIgnore);
			else
				result.DetectedHero = detectedHero;

			//move
			Move(gameState, result);
			Spawn (gameState, result);

			return result;
		}

		private void Detect(GameState gameState, NecromancerActivationResult result,
			int[] heroesToIgnore){
			//first check if any heroes can be detected
			var exposedHeroes = gameState.Heroes.Active.Where(x => x.LocationId != LocationIds.Monastery && x.Secrecy < result.DetectionRoll && (heroesToIgnore == null || !heroesToIgnore.Contains(x.Id)));

			//special hero effects
			if (gameState.Heroes.HermitActive)
				exposedHeroes = exposedHeroes.Where(x => !(x.Name == "Ranger" && x.LocationId == LocationIds.Swamp));

			if (gameState.Heroes.AuraOfHumilityActive) {
				var paragonLocationId = gameState.Heroes.Active.First (x => x.Name == "Paragon").LocationId;
				exposedHeroes = exposedHeroes.Where (x => x.LocationId != paragonLocationId);
			}

			if (exposedHeroes.Any())
			{
				//figure out which hero the necromancer is pursuing

				if (gameState.Necromancer.GatesActive) //if gates are active pick a random hero
				{
					var index = new Random().Next(0, exposedHeroes.Count() - 1);
					result.DetectedHero= exposedHeroes.ToArray()[index];
				}
				else if (exposedHeroes.Any(x => x.LocationId == gameState.Necromancer.LocationId))
				{
					result.DetectedHero= exposedHeroes.First(x => x.LocationId == gameState.Necromancer.LocationId);
				}
				else
				{
					//TODO: this needs work. not exactly readable at the moment
					var necromancerLocation = gameState.Locations.First(x => x.Id == gameState.Necromancer.LocationId);

					var heroesOneLocationAway = exposedHeroes.Where(x => necromancerLocation.Pathways.Any(y => y == x.LocationId));
					if(heroesOneLocationAway.Any())
						exposedHeroes = heroesOneLocationAway;

					var index = new Random().Next(0, exposedHeroes.Count() - 1);
					result.DetectedHero= exposedHeroes.ToArray () [index];
				}
			}
			else
				result.DetectedHero= null;
		}

		private void Move(GameState gameState, NecromancerActivationResult result)
        {
            var currentLocation = gameState.Locations.Single(x => x.Id == gameState.Necromancer.LocationId);

            if (result.DetectedHero != null)
            {
				var heroLocation = gameState.Locations.Single(x => x.Id == result.DetectedHero.LocationId);
                var necromancerLocation = gameState.Locations.First(x => x.Id == gameState.Necromancer.LocationId);
                //if the necromancer can reach the hero go directly to them
                if (necromancerLocation.Pathways.Contains(heroLocation.Id) || gameState.Necromancer.GatesActive)
					result.NewLocation = heroLocation;
                else
                {
                    //the common locations between the hero and necromancer are the ones that the necro should move along
                    var commonLocationIds = necromancerLocation.Pathways.Intersect(heroLocation.Pathways).ToArray();
                    var index = new Random().Next(0, commonLocationIds.Length - 1);
					result.NewLocation = gameState.Locations.Single(x=> x.Id == commonLocationIds[index]);
                }
            }
            else
				result.NewLocation = gameState.Locations.Single(x=>x.Id == currentLocation.Pathways[result.MovementRoll - 1]);
        }

		private void Spawn(GameState gameState, NecromancerActivationResult result)
        {
            //spawn blights at necromancer's new location
            result.NumberOfBlightsToNewLocation = 1;

            //standard darkness track effects
            if (gameState.DarknessTrackEffectsActive)
            {
				if (gameState.DarknessLevel >= 10 && result.NewLocation.NumberOfBlights == 0)
                    result.NumberOfBlightsToNewLocation++;

				if (gameState.DarknessLevel >= 20 && (result.MovementRoll == 1 || result.MovementRoll == 2))
                    result.NumberOfBlightsToNewLocation++;
            }

            //darkness card effects
            if (gameState.Mode != DarknessCardsMode.None)
            {
                if (gameState.Necromancer.FocusedRituals
                    && !gameState.Heroes.Active.Any(x => x.LocationId == result.NewLocation.Id))
                    result.NumberOfBlightsToNewLocation++;

                if (gameState.Necromancer.CreepingShadows && (result.MovementRoll == 5 || result.MovementRoll == 6))
                    result.NumberOfBlightsToNewLocation++;

                if (gameState.Necromancer.DyingLand)
                {
                    if (gameState.DarknessTrackEffectsActive
                        && gameState.DarknessLevel >= 10
                        && result.NewLocation.NumberOfBlights == 1)
                        result.NumberOfBlightsToNewLocation++;
                    else if ((!gameState.DarknessTrackEffectsActive
                        || gameState.DarknessLevel < 10)
                        && result.NewLocation.NumberOfBlights == 0)
                        result.NumberOfBlightsToNewLocation++;
                }

                if (gameState.Necromancer.EncroachingShadows && result.MovementRoll == 6)
                    result.NumberOfBlightsToMonastery++;

                if (gameState.Necromancer.Overwhelm && result.NewLocation.NumberOfBlights < 4 && result.NewLocation.NumberOfBlights + result.NumberOfBlightsToNewLocation >= 4)
                    result.NumberOfBlightsToMonastery++;
            }

            //check for spill over to monastery
            if (result.NewLocation.NumberOfBlights + result.NumberOfBlightsToNewLocation > 4)
            {
                var overflow = (result.NewLocation.NumberOfBlights + result.NumberOfBlightsToNewLocation) - 4;
                result.NumberOfBlightsToNewLocation -= overflow;
                result.NumberOfBlightsToMonastery += overflow;
            }
            
            //check if a quest needs to be spawned
            if (gameState.PallOfSuffering && (result.MovementRoll == 3 || result.MovementRoll == 4))
                result.SpawnQuest = true;
        }
    }

    public class DetectionResult
    {
        public Hero DetectedHero { get; set; }
        public int MovementRoll { get; set; }
		public int DetectionRoll {get;set;}
    }


    public class SpawnResult
    {
        public int NumberOfBlightsToNewLocation { get; set; }
        public int NumberOfBlightsToMonastery { get; set; }

        public bool SpawnQuest { get; set; }
    }

	public class NecromancerActivationResult{
		public Hero DetectedHero { get; set; }
		public int MovementRoll { get; set; }
		public int DetectionRoll {get;set;}
		public Location NewLocation { get; set; }
		public int NumberOfBlightsToNewLocation { get; set; }
		public int NumberOfBlightsToMonastery { get; set; }

		public bool SpawnQuest { get; set; }	
	}
}
