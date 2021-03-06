﻿using System;
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

		public GameState CreateGameState (int numberOfPlayers, int startingDarkness, int startingBlights, bool useQuests, DarknessCardsMode mode)//DifficultyLevelSettings difficultyLevelSettings)
		{
			var gameState = new GameState ();

			gameState.NumberOfPlayers = numberOfPlayers;
			//gameState.DifficultyLevel = difficultyLevelSettings.DifficultyLevel;
			gameState.Darkness = startingDarkness;
			gameState.UseQuests = useQuests;
			//gameState.SpawnExtraQuests = difficultyLevelSettings.SpawnExtraQuests;
			gameState.Mode = mode;

			gameState.Necromancer.LocationId = (int)LocationIds.Ruins;

			gameState.MapCards = new Deck<MapCard> ();
			gameState.MapCards.Initialise (_dataService.GetMapCards ());

			gameState.BlightPool = _dataService.GetBlights ();

			gameState.Locations = _dataService.GetLocations();

			_blightService.SpawnStartingBlights (startingBlights, gameState);

			return gameState;
		}
	}
}

