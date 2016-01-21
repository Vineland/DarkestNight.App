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

namespace Vineland.Necromancer.UI
{
	public class SaveGameService
	{
		FileService _fileService;
		AppSettings _appSettings;

		public SaveGameService(FileService fileService, AppSettings appSettings)
		{
			_fileService = fileService;
			_appSettings = appSettings;
		}

		public GameState CreateDefaultGame()
		{			
			var gameState = new GameState();
			gameState.CreatedDate = DateTime.Now;
			gameState.DarknessLevel = _appSettings.StartingDarkness;
			gameState.PallOfSuffering = _appSettings.PallOfSuffering;
			gameState.Mode = _appSettings.DarknessCardsMode;

			gameState.Necromancer.LocationId = LocationIds.Ruins;
			gameState.Locations.ForEach (x => x.NumberOfBlights = 1);
			gameState.Locations [0].NumberOfBlights = 0;

			return gameState;
		}

		public void Save(GameState gameState)
		{
			Task.Run (() => {
				var path = Path.Combine (AppConstants.SavesLocation, gameState.CreatedDate.GetHashCode () + AppConstants.SaveFileExtension);
				_fileService.SaveFile (path, JsonConvert.SerializeObject (this));
			});
		}

		public GameState Load(FileInfo save)
		{
			return JsonConvert.DeserializeObject<GameState>(_fileService.LoadFile(save));
		}
	}
}

