using System;
using System.Linq;
using Vineland.Necromancer.Domain;
using Vineland.Necromancer.Core.Models;

namespace Vineland.Necromancer.Core
{
	public class BlightService
	{
		public BlightService ()
		{
		}

		public void SpawnStartingBlights(GameState gameState)
		{
			foreach (var location in gameState.Locations)
				location.Blights.Clear();

			if (gameState.DifficultyLevel.Id == DifficultyLevelId.Page)
				SpawnBlight(LocationId.Ruins, gameState);
			else 
			{
				for (int i = 0; i < gameState.DifficultyLevel.StartingBlights; i++)
				{
					var mapCard = gameState.MapCards.Draw();

					foreach (var row in mapCard.Rows)
					{
						if (row.LocationId == LocationId.Monastery)
							continue;

						var blight = gameState.BlightPool.FirstOrDefault(x => x.Name == row.BlightName);
						gameState.Locations.Single(l => l.Id == row.LocationId).Blights.Add(blight);
						gameState.BlightPool.Remove(blight);
					}

					gameState.MapCards.Discard(mapCard);
				}
			}
		}

		/// <summary>
		/// Spawns a blight at the specified locaiton. 
		/// If the location is already at capacity then it spawns a blight at the monastery instead.
		/// </summary>
		/// <returns>A tuple containting the blight spawned and the location spawned at.</returns>
		/// <param name="location">Location.</param>
		/// <param name="gameState">Game state.</param>
		public SpawnBlightResult SpawnBlight (LocationId locationId, GameState gameState)
		{
			var location = gameState.Locations.Single(l => l.Id == locationId);
			if (location.BlightCount >= 4)
				location = gameState.Locations.Single (l => l.Id == LocationId.Monastery);
			
			var card = gameState.MapCards.Draw ();
			var blightName = card.Rows.Single (r => r.LocationId == location.Id).BlightName;
			var blight = gameState.BlightPool.FirstOrDefault (x => x.Name == blightName);
			gameState.MapCards.Discard (card);

			if (blight != null) {
				location.Blights.Add (blight);
				gameState.BlightPool.Remove (blight);
			} else //if there are no blights of that type left to spawn try the next card
				return SpawnBlight (locationId, gameState);

			return new SpawnBlightResult() { Location = location, Blight = blight };
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

