using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vineland.DarkestNight.Core.Services;

namespace Vineland.DarkestNight.Core
{
    public class NecromancerService
    {
        GameData _gameData;
        D6GeneratorService _d6GeneratorService;

        public NecromancerService(GameData gameData, D6GeneratorService d6GeneratorService)
        {
            _gameData = gameData;
            _d6GeneratorService = d6GeneratorService;
        }

        public DetectionResult Detect(GameState gameState)
        {
            var result = new DetectionResult();

            result.MovementRoll = _d6GeneratorService.RollDemBones();
            var detectionRoll = result.MovementRoll;

            if (gameState.Necromancer.GatesActive)
                detectionRoll++;

            //first check if any heroes can be detected
            var exposedHeroes = gameState.Heroes.Active.Where(x => x.LocationId != LocationIds.Monastery && x.Secrecy < detectionRoll);

            //special hero effects
            if (gameState.Heroes.HermitActive)
                exposedHeroes = exposedHeroes.Where(x => !(x.Name == "Ranger" && x.LocationId == LocationIds.Swamp));

            if (gameState.Heroes.AuraOfHumilityLocationId.HasValue)
                exposedHeroes = exposedHeroes.Where(x => x.LocationId != gameState.Heroes.AuraOfHumilityLocationId.Value);

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

        public MoveResult Move(int? detectedHeroId, int movementRoll, GameState gameState)
        {
            var result = new MoveResult();

            var currentLocation = gameState.Locations.First(x => x.Id == gameState.Necromancer.LocationId);
            var newLocationId = 0;

            if (detectedHeroId.HasValue)
            {
                //TODO
            }
            else
                newLocationId = currentLocation.Pathways[movementRoll - 1];
            
            result.NewNecromancerLocation = gameState.Locations.First(x => x.Id == newLocationId);

            //spawn blights at necromancer's new location
            result.NumberOfBlightsToNewLocation = 1;

            //standard darkness track effects
            if (gameState.DarknessTrackEffectsActive)
            {
                if (gameState.DarknessLevel >= 10 && result.NewNecromancerLocation.NumberOfBlights == 0)
                    result.NumberOfBlightsToNewLocation++;

                if (gameState.DarknessLevel >= 20 && (movementRoll == 1 || movementRoll == 2))
                    result.NumberOfBlightsToNewLocation++;
            }

            //darkness card effects
            if (gameState.Mode != DarknessCardsMode.None)
            {
                if (gameState.Necromancer.FocusedRituals
                    && !gameState.Heroes.Active.Any(x => x.LocationId == newLocationId))
                    result.NumberOfBlightsToNewLocation++;

                if (gameState.Necromancer.CreepingShadows && (movementRoll == 5 || movementRoll == 6))
                    result.NumberOfBlightsToNewLocation++;

                if (gameState.Necromancer.DyingLand)
                {
                    if (gameState.DarknessTrackEffectsActive
                        && gameState.DarknessLevel >= 10
                        && result.NewNecromancerLocation.NumberOfBlights == 0)
                        result.NumberOfBlightsToNewLocation++;
                    else if (result.NewNecromancerLocation.NumberOfBlights == 1)
                        result.NumberOfBlightsToNewLocation++;
                }

                if (gameState.Necromancer.EncroachingShadows && movementRoll == 6)
                    result.NumberOfBlightsToMonastery++;

                if (gameState.Necromancer.Overwhelm && result.NewNecromancerLocation.NumberOfBlights + result.NumberOfBlightsToNewLocation >= 4)
                    result.NumberOfBlightsToMonastery++;
            }

            //check for spill over to monastery
            if (result.NewNecromancerLocation.NumberOfBlights + result.NumberOfBlightsToNewLocation > 4)
            {
                var overflow = (result.NewNecromancerLocation.NumberOfBlights + result.NumberOfBlightsToNewLocation) - 4;
                result.NumberOfBlightsToNewLocation -= overflow;
                result.NumberOfBlightsToMonastery += overflow;

            }


            //result.Messages.Add(string.Format("{0} blight{2} spawned at the {1}", numberOfBlights, result.NewNecromancerLocation.Name, numberOfBlights > 1 ? "s are" : "is"));
            //result.Messages.Add(string.Format("{0} blight{1} spawned at the Monastery", numberOfBlightsMonastery, numberOfBlightsMonastery > 1 ? "s are" : "is"));

            //check if a quest needs to be spawned
            if (gameState.PallOfSuffering && (movementRoll == 3 || movementRoll == 4))
                result.SpawnQuest = true;
                //result.Messages.Add(string.Format("A Quest is spawned at the {0}", currentLocation.Name));

            
            return result;
        }
    }

    public class DetectionResult
    {
        public int? DetectedHeroId { get; set; }
        public int MovementRoll { get; set; }
    }


    public class MoveResult
    {
        public Location NewNecromancerLocation { get; set; }
        //public List<string> Messages { get; set; }
        /// <summary>
        /// A list of (LocationId, NumberOfBlights)
        /// </summary>
        public int NumberOfBlightsToNewLocation { get; set; }
        public int NumberOfBlightsToMonastery { get; set; }

        public bool SpawnQuest { get; set; }
    }

}
