using Vineland.Necromancer.Domain;
using System.Linq;
using Newtonsoft.Json;
using Vineland.Necromancer.Core.Services;

namespace Vineland.Necromancer.Core
{
	/// <summary>
	/// Responsible for creating and maintaining the state of the game.
	/// </summary>
	public class GameStateService
	{
		DataService _dataService;
		BlightService _blightService;
		QuestService _questService;
		IFileService _fileService;

		public GameStateService(DataService dataService,
								 BlightService blightService,
		                        QuestService questService,
		                        IFileService fileService)
		{
			_dataService = dataService;
			_blightService = blightService;
			_fileService = fileService;
			_questService = questService;
		}

		public GameState StartNewGame(int numberOfPlayers, DifficultyLevel difficultyLevel)
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

			_blightService.SpawnStartingBlights(gameState);

			_questService.SpawnStartingQuests(gameState);

			return gameState;
		}

		public GameState LoadGame(string savePath)
		{
			if (_fileService.DoesFileExist(savePath))
				return JsonConvert.DeserializeObject<GameState>(_fileService.LoadFile(savePath), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

			return null;
		}

		public void SaveCurrentGame(string savePath, GameState gameState)
		{
			var gameJson = JsonConvert.SerializeObject(gameState, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
			_fileService.SaveFile(savePath, gameJson);
		}
	}
}
