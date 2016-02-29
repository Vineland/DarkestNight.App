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

		public void SpawnStartingBlights (GameState gameState)
		{
			var mapCard = gameState.MapCards.Draw();
			foreach (var row in mapCard.Rows) {
				if (row.Location == "Monastery")
					continue;
				
				var blight = gameState.BlightPool.FirstOrDefault (x => x.Name == row.Blight);
				gameState.Locations.Single (l => l.Name == row.Location).Blights.Add (blight);
				gameState.BlightPool.Remove (blight);
			}
		}

		public Blight SpawnBlight (Location location, GameState gameState)
		{
			var blightName = gameState.MapCards.Draw().Rows.Single (r => r.Location == location.Name).Blight;
			var blight = gameState.BlightPool.FirstOrDefault (x => x.Name == blightName);
			if (blight != null) {
				location.Blights.Add (blight);
				gameState.BlightPool.Remove (blight);
			} else //if there are no blights of that type left to spawn try the next card
				SpawnBlight (location, gameState);

			return blight;
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

