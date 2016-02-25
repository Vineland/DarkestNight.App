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

		public GameState CreateGameState (Settings settings)
		{
			var gameState = new GameState ();

			gameState.Darkness = settings.StartingDarkness;
			gameState.PallOfSuffering = settings.PallOfSuffering;
			gameState.Mode = settings.DarknessCardsMode;

			gameState.Necromancer.LocationId = LocationIds.Ruins;

			gameState.MapCards = new Deck<MapCard> ();
			gameState.MapCards.Initialise (_dataService.GetMapCards ());

			gameState.BlightPool = _dataService.GetBlights ();

			gameState.Locations = new List<Location> (Location.All);

			_blightService.SpawnStartingBlights (gameState);

			return gameState;
		}
	}
}

