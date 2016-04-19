using System;
using System.Collections.Generic;
using System.Linq;

namespace Vineland.Necromancer.Core
{
	public class GameStateFactory
	{
		DataService _dataService;
		BlightService _blightService;

		public GameStateFactory (DataService dataService, 
		                         BlightService blightService)
		{
			_dataService = dataService;
			_blightService = blightService;
		}

		public GameState CreateGameState (DifficultyLevelSettings difficultyLevelSettings)
		{
			var gameState = new GameState ();

			gameState.Darkness = difficultyLevelSettings.StartingDarkness;
			gameState.PallOfSuffering = difficultyLevelSettings.PallOfSuffering;
			gameState.SpawnExtraQuests = difficultyLevelSettings.SpawnExtraQuests;
			//gameState.Mode = settings.DarknessCardsMode;

			gameState.Necromancer.LocationId = (int)LocationIds.Ruins;

			gameState.MapCards = new Deck<MapCard> ();
			gameState.MapCards.Initialise (_dataService.GetMapCards ());

			gameState.BlightPool = _dataService.GetBlights ();

			gameState.Locations = _dataService.GetLocations();

			_blightService.SpawnStartingBlights (difficultyLevelSettings, gameState);

			return gameState;
		}
	}
}

