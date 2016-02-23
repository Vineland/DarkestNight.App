using System;
using Vineland.DarkestNight.UI.Services;
using System.IO;
using Vineland.DarkestNight.UI;
using Newtonsoft.Json;
using Vineland.Necromancer.Core;
using Android.Util;
using System.Linq;
using Java.Lang;
using System.Threading.Tasks;
using Org.Apache.Http.Impl.Client;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class GameStateService
	{
		FileService _fileService;

		public GameState CurrentGame { get; private set; }

		public GameStateService (FileService fileService)
		{
			_fileService = fileService;

			if (_fileService.DoesFileExist (AppConstants.SaveFilePath))
				Continue ();
		}

		public void NewGame (Settings appSettings)
		{			
			var gameState = new GameState ();
			gameState.CreatedDate = DateTime.Now;
			gameState.Darkness = appSettings.StartingDarkness;
			gameState.PallOfSuffering = appSettings.PallOfSuffering;
			gameState.Mode = appSettings.DarknessCardsMode;

			gameState.Necromancer.LocationId = LocationIds.Ruins;
			gameState.Locations = new List<Location> (Location.All);
			gameState.Locations.ForEach (x => x.NumberOfBlights = 1);
			gameState.Locations [0].NumberOfBlights = 0;

			CurrentGame = gameState;
		}

		public async void Save ()
		{
			await Task.Run (() => {
				_fileService.SaveFile (AppConstants.SaveFilePath, JsonConvert.SerializeObject (CurrentGame));
			});
		}

		public void Continue ()
		{
			CurrentGame = JsonConvert.DeserializeObject<GameState> (_fileService.LoadFile (AppConstants.SaveFilePath));
		}
	}
}

