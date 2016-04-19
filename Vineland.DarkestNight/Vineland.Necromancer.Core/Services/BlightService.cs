using System;
using System.Linq;
using Vineland.Necromancer.Core;
using Newtonsoft.Json.Schema;

namespace Vineland.Necromancer.Core
{
	public class BlightService
	{
		public BlightService ()
		{
		}

		public void SpawnStartingBlights (DifficultyLevelSettings difficultyLevelSettings, GameState gameState)
		{
			if (difficultyLevelSettings.DifficultyLevel == DifficultyLevel.Page) 
			{
				var mapCard = gameState.MapCards.Draw ();
				var blightName = mapCard.Rows.Single (x => x.LocationId == (int)LocationIds.Ruins).BlightName;
				var blight = gameState.BlightPool.FirstOrDefault (x => x.Name == blightName);
				gameState.Locations.Single (l => l.Id == (int)LocationIds.Ruins).Blights.Add (blight);
				gameState.BlightPool.Remove (blight);

			} else {
				for (int i = 0; i < difficultyLevelSettings.StartingBlights; i++) {
					var mapCard = gameState.MapCards.Draw ();

					foreach (var row in mapCard.Rows) {
						if (row.LocationId == (int)LocationIds.Monastery)
							continue;
				
						var blight = gameState.BlightPool.FirstOrDefault (x => x.Name == row.BlightName);
						gameState.Locations.Single (l => l.Id == row.LocationId).Blights.Add (blight);
						gameState.BlightPool.Remove (blight);
					}

					gameState.MapCards.Discard (mapCard);
				}
			}
		}

		/// <summary>
		/// Spawns a blight at the specified locaiton. 
		/// If the location is already at capacity then it spawns a blight at the monastery instead.
		/// </summary>
		/// <returns>A typle containting the blight spawned and the locaiton spawned at.</returns>
		/// <param name="location">Location.</param>
		/// <param name="gameState">Game state.</param>
		public Tuple<Location, Blight> SpawnBlight (Location location, GameState gameState)
		{
			if (location.BlightCount >= 4)
				location = gameState.Locations.Single (l => l.Id == (int)LocationIds.Monastery);
			
			var card = gameState.MapCards.Draw ();
			var blightName = card.Rows.Single (r => r.LocationId == location.Id).BlightName;
			var blight = gameState.BlightPool.FirstOrDefault (x => x.Name == blightName);
			gameState.MapCards.Discard (card);

			if (blight != null) {
				location.Blights.Add (blight);
				gameState.BlightPool.Remove (blight);
			} else //if there are no blights of that type left to spawn try the next card
				return SpawnBlight (location, gameState);

			return new Tuple<Location, Blight>(location, blight);
		}

		public void DestroyBlight (Location location, Blight blight, GameState gameState)
		{
			if (location.Blights.Contains (blight)) {
				location.Blights.Remove (blight);
				gameState.BlightPool.Add (blight);
			}
		}
	}
}

