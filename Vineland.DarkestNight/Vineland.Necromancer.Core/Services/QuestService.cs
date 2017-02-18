using System;
using Vineland.Necromancer.Core.Services;
using Vineland.Necromancer.Domain;
using System.Linq;

namespace Vineland.Necromancer.Core.Services
{
	public class QuestService
	{
		D6GeneratorService _d6Generator;

		public QuestService(D6GeneratorService d6Generator)
		{
			_d6Generator = d6Generator;
		}

		public void SpawnStartingQuests(GameState gameState)
		{
			
			switch (gameState.DifficultyLevel.Id) {
			case DifficultyLevelId.Champion:
			case DifficultyLevelId.Heroic:
					SpawnQuest(LocationId.Village, gameState);
				break;
			case DifficultyLevelId.Legendary:
					SpawnRandomQuest (gameState);
					SpawnRandomQuest (gameState);
				break;
			}
		}

		public void SpawnQuest(LocationId locationId, GameState gameState)
		{
			var location = gameState.Locations.Single(x => x.Id == locationId);
			location.Quests.Add(new Quest());
		}

		public void SpawnRandomQuest(GameState gameState)
		{
			var locationId = _d6Generator.RollDemBones();
			var location = gameState.Locations.Single(x => x.Id == (LocationId)locationId);
			location.Quests.Add(new Quest());
		}
	}
}
