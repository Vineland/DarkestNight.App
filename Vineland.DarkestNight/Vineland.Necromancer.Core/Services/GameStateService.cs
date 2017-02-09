using System;
using Vineland.Necromancer.Domain;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Vineland.Necromancer.Core
{
	/// <summary>
	/// Responsible for creating and maintaining the state of the game.
	/// </summary>
	public class GameStateService
	{
		DataService _dataService;
		BlightService _blightService;
		IFileService _fileService;

		public GameStateService(DataService dataService,
								 BlightService blightService,
		                        IFileService fileService)
		{
			_dataService = dataService;
			_blightService = blightService;
			_fileService = fileService;
		}

		public GameState CurrentGame
		{
			get; private set;
		}

		public void StartNewGame(int numberOfPlayers, DifficultyLevel difficultyLevel)
		{
			var gameState = new GameState();

			gameState.NumberOfPlayers = numberOfPlayers;
			gameState.DifficultyLevel = difficultyLevel;
			gameState.Darkness = difficultyLevel.StartingDarkness;

			gameState.Necromancer.LocationId = LocationId.Ruins;

			gameState.MapCards = new Deck<MapCard>();
			gameState.MapCards.Initialise(_dataService.GetMapCards().ToList());

			gameState.BlightPool = _dataService.GetBlights().ToList();

			gameState.Locations = _dataService.GetLocations().ToList();

			if (difficultyLevel.Id == DifficultyLevelId.Page)
				_blightService.SpawnBlight(LocationId.Ruins, gameState);
			else
				_blightService.SpawnStartingBlights(difficultyLevel.StartingBlights, gameState);

			CurrentGame = gameState;
		}

		public void LoadGame(string savePath)
		{
			if (_fileService.DoesFileExist(savePath))
				CurrentGame = JsonConvert.DeserializeObject<GameState>(_fileService.LoadFile(savePath), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
		}

		public void SaveCurrentGame(string savePath)
		{
			var gameJson = JsonConvert.SerializeObject(CurrentGame, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
			_fileService.SaveFile(savePath, gameJson);
		}
	}
}
