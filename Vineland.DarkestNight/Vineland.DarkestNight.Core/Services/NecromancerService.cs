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

        public NecromancerResult ActivateNecromancer(GameState gameState)
        {
            var result = new NecromancerResult() { Messages = new List<string>() };

            var dieRoll = _d6GeneratorService.RollDemBones();

            var newLocationId = 0;
            //first check if any heroes can be detected
            var exposedHeroes = gameState.Heroes.Where(x => x.LocationId != 1 && x.Secrecy < dieRoll);
            if (exposedHeroes.Any())
            {
                //figure out which hero the necromancer is pursuing
            }
            else
            {
                //cannot detect anyone so move along the pathways
                newLocationId = gameState.NecromancerLocation.Pathways[dieRoll - 1];
            }

            if (newLocationId == gameState.NecromancerLocation.Id)
            {
                result.NewNecromancerLocation = gameState.NecromancerLocation;
                result.Messages.Add(string.Format("The Necromancer remains at the {0}", gameState.NecromancerLocation.Name));
            }
            else
            {
                result.NewNecromancerLocation = _gameData.Locations.First(x => x.Id == newLocationId);
                result.Messages.Add(string.Format("The Necromancer moves to the {0}", result.NewNecromancerLocation.Name));
            }

            //spawn blights at necromancer's new location
            var numberOfBlights = 1;
            var numberOfBlightsMonastery = 0;
            //standard darkness track effects
            if (gameState.DarknessTrackEffectsActive)
            {
                if (gameState.DarknessLevel >= 10 && result.NewNecromancerLocation.NumberOfBlights == 0)
                    numberOfBlights++;

                if (gameState.DarknessLevel >= 20 && (dieRoll == 1 || dieRoll == 2))
                    numberOfBlights++;
            }

            //darkness card effects
            if (gameState.Mode != DarknessCardsMode.None)
            {
                if (gameState.DarknessCards.FocusedRituals
                    && !gameState.Heroes.Any(x => x.LocationId == newLocationId))
                    numberOfBlights++;

                if (gameState.DarknessCards.CreepingShadows && (dieRoll == 5 || dieRoll == 6))
                    numberOfBlights++;

                if (gameState.DarknessCards.DyingLand)
                {
                    if (gameState.DarknessTrackEffectsActive
                        && gameState.DarknessLevel >= 10
                        && result.NewNecromancerLocation.NumberOfBlights == 0)
                        numberOfBlights++;
                    else if (result.NewNecromancerLocation.NumberOfBlights == 1)
                        numberOfBlights++;
                }

                if (gameState.DarknessCards.EncroachingShadows && dieRoll == 6)
                    numberOfBlightsMonastery++;

                if (gameState.DarknessCards.Overwhelm && result.NewNecromancerLocation.NumberOfBlights + numberOfBlights >= 4)
                    numberOfBlightsMonastery++;
            }

            //check for spill over to monastery
            if (result.NewNecromancerLocation.NumberOfBlights + numberOfBlights > 4)
            {
                var overflow = (result.NewNecromancerLocation.NumberOfBlights + numberOfBlights) - 4;
                numberOfBlights -= overflow;
                numberOfBlightsMonastery += overflow;
                
            }
          

            result.Messages.Add(string.Format("{0} blight{2} spawned at the {1}", numberOfBlights, result.NewNecromancerLocation.Name, numberOfBlights > 1 ? "s are": "is"));
            result.Messages.Add(string.Format("{0} blight{1} spawned at the Monastery", numberOfBlightsMonastery, numberOfBlightsMonastery > 1 ? "s are" : "is"));

            //check if a quest needs to be spawned
            if (gameState.PallOfSuffering && (dieRoll == 3 || dieRoll == 4))
                result.Messages.Add(string.Format("A Quest is spawned at the {0}", gameState.NecromancerLocation.Name));



            return result;
        }
    }

    public class NecromancerResult
    {
        public Location NewNecromancerLocation { get; set; }
        public List<string> Messages { get; set; }
        /// <summary>
        /// A list of (LocationId, NumberOfBlights)
        /// </summary>
        public List<Tuple<int, int>> NumberOfBlights { get; set; }
        public bool SpawnQuest { get; set; }
    }

}
