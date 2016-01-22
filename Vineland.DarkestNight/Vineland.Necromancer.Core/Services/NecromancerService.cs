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

		public DetectionResult Detect(GameState gameState, int? roll = null, int[] heroesToIgnore = null)
        {
            var result = new DetectionResult();

			result.MovementRoll = roll.HasValue ? roll.Value : _d6GeneratorService.RollDemBones();
			result.DetectionRoll = result.MovementRoll;

            if (gameState.Necromancer.GatesActive)
				result.DetectionRoll++;

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
                    result.DetectedHeroId = exposedHeroes.ToArray()[index].Id;
                }
                else if (exposedHeroes.Any(x => x.LocationId == gameState.Necromancer.LocationId))
                {
                    result.DetectedHeroId = exposedHeroes.First(x => x.LocationId == gameState.Necromancer.LocationId).Id;
                }
                else
                {
                    //TODO: this needs work. not exactly readable at the moment
                    var necromancerLocation = gameState.Locations.First(x => x.Id == gameState.Necromancer.LocationId);

                    var heroesOneLocationAway = exposedHeroes.Where(x => necromancerLocation.Pathways.Any(y => y == x.LocationId));
                    if(heroesOneLocationAway.Any())
                        exposedHeroes = heroesOneLocationAway;

                    var index = new Random().Next(0, exposedHeroes.Count() - 1);
                    result.DetectedHeroId = exposedHeroes.ToArray()[index].Id;
                }
            }
            else
                result.DetectedHeroId = null;

            return result;
        }

        public int Move(Hero detectedHero, int movementRoll, GameState gameState)
        {
            var currentLocation = gameState.Locations.First(x => x.Id == gameState.Necromancer.LocationId);

            if (detectedHero != null)
            {
                var heroLocation = gameState.Locations.First(x => x.Id == detectedHero.LocationId);
                var necromancerLocation = gameState.Locations.First(x => x.Id == gameState.Necromancer.LocationId);
                //if the necromancer can reach the hero go directly to them
                if (necromancerLocation.Pathways.Contains(heroLocation.Id) || gameState.Necromancer.GatesActive)
                    return heroLocation.Id;
                else
                {
                    //the common locations between the hero and necromancer are the ones that the necro should move along
                    var commonLocationIds = necromancerLocation.Pathways.Intersect(heroLocation.Pathways).ToArray();
                    var index = new Random().Next(0, commonLocationIds.Length - 1);
                    return commonLocationIds[index];
                }
            }
            else
                return currentLocation.Pathways[movementRoll - 1];
        }

        public SpawnResult Spawn(Location newLocation, int movementRoll, GameState gameState)
        {
            var result = new SpawnResult();

            //spawn blights at necromancer's new location
            result.NumberOfBlightsToNewLocation = 1;

            //standard darkness track effects
            if (gameState.DarknessTrackEffectsActive)
            {
                if (gameState.DarknessLevel >= 10 && newLocation.NumberOfBlights == 0)
                    result.NumberOfBlightsToNewLocation++;

                if (gameState.DarknessLevel >= 20 && (movementRoll == 1 || movementRoll == 2))
                    result.NumberOfBlightsToNewLocation++;
            }

            //darkness card effects
            if (gameState.Mode != DarknessCardsMode.None)
            {
                if (gameState.Necromancer.FocusedRituals
                    && !gameState.Heroes.Active.Any(x => x.LocationId == newLocation.Id))
                    result.NumberOfBlightsToNewLocation++;

                if (gameState.Necromancer.CreepingShadows && (movementRoll == 5 || movementRoll == 6))
                    result.NumberOfBlightsToNewLocation++;

                if (gameState.Necromancer.DyingLand)
                {
                    if (gameState.DarknessTrackEffectsActive
                        && gameState.DarknessLevel >= 10
                        && newLocation.NumberOfBlights == 1)
                        result.NumberOfBlightsToNewLocation++;
                    else if ((!gameState.DarknessTrackEffectsActive
                        || gameState.DarknessLevel < 10)
                        && newLocation.NumberOfBlights == 0)
                        result.NumberOfBlightsToNewLocation++;
                }

                if (gameState.Necromancer.EncroachingShadows && movementRoll == 6)
                    result.NumberOfBlightsToMonastery++;

                if (gameState.Necromancer.Overwhelm && newLocation.NumberOfBlights < 4 && newLocation.NumberOfBlights + result.NumberOfBlightsToNewLocation >= 4)
                    result.NumberOfBlightsToMonastery++;
            }

            //check for spill over to monastery
            if (newLocation.NumberOfBlights + result.NumberOfBlightsToNewLocation > 4)
            {
                var overflow = (newLocation.NumberOfBlights + result.NumberOfBlightsToNewLocation) - 4;
                result.NumberOfBlightsToNewLocation -= overflow;
                result.NumberOfBlightsToMonastery += overflow;
            }
            
            //check if a quest needs to be spawned
            if (gameState.PallOfSuffering && (movementRoll == 3 || movementRoll == 4))
                result.SpawnQuest = true;

            
            return result;
        }
    }

    public class DetectionResult
    {
        public int? DetectedHeroId { get; set; }
        public int MovementRoll { get; set; }
		public int DetectionRoll {get;set;}
    }


    public class SpawnResult
    {
        public int NumberOfBlightsToNewLocation { get; set; }
        public int NumberOfBlightsToMonastery { get; set; }

        public bool SpawnQuest { get; set; }
    }

}
