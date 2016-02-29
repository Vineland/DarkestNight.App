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
				if (row.LocationId == LocationIds.Monastery)
					continue;
				
				var blight = gameState.BlightPool.FirstOrDefault (x => x.Name == row.BlightName);
				gameState.Locations.Single (l => l.Id == row.LocationId).Blights.Add (blight);
				gameState.BlightPool.Remove (blight);
			}
		}

		public Blight SpawnBlight (Location location, GameState gameState, Blight blight = null)
		{
			//TODO: look into this - sometimes the draw has to be done seperately 
			//(during the necromancer phase where the player can interrupt in various ways)
			//and in those cases the blight will be passed in after it has been confirmed
			//otherwise we can draw and spawn in one method. however, this resulting code is a bit messy
			if(blight == null){
				var card = gameState.MapCards.Draw ();
				var blightName = card.Rows.Single (r => r.LocationId == location.Id).BlightName;
				blight = gameState.BlightPool.FirstOrDefault (x => x.Name == blightName);
				gameState.MapCards.Discard (card);
			}

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

