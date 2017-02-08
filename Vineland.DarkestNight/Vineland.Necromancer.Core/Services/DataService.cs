using System;
using System.Collections.Generic;
using System.Linq;
using Vineland.Necromancer.Domain;
using Vineland.Necromancer.Repository;

namespace Vineland.Necromancer.Core
{
	public class DataService
	{	
		Settings _settings;
		IRepository _repository;

		public DataService(IRepository repository, Settings settings)
		{
			_settings = settings;
			_repository = repository;
		}

		public IEnumerable<Blight> GetBlights()
		{
			var blights = _repository.GetAll<Blight>().Where(x => _settings.Expansions.HasFlag(x.Expansion));

			return blights.OrderBy(x => x.Name).ToList();
		}

		public IEnumerable<MapCard> GetMapCards()
		{
			var mapCards = _repository.GetAll<MapCard>().Where(x => _settings.Expansions.HasFlag(x.Expansion)).ToList();

			mapCards.Shuffle();

			return mapCards;
		}

		public IEnumerable<Hero> GetAllHeroes()
		{
			var heroes = _repository.GetAll<Hero>().Where(x => _settings.Expansions.HasFlag(x.Expansion));

			return heroes.OrderBy(x => x.Name).ToList();
		}


		public IEnumerable<Location> GetLocations()
		{
			return _repository.GetAll<Location>();
		}

		public List<DifficultyLevelSettings> GetDifficultyLevelSettings()
		{
			var difficultyLevelSettings = new List<DifficultyLevelSettings>();
			difficultyLevelSettings.Add(new DifficultyLevelSettings()
			{
				DifficultyLevel = DifficultyLevel.Page,
				StartingBlights = 0,
				StartingDarkness = 0,
				Notes = string.Format("+2 to time limits{0}Start with 1 spark each{0}Start with 1 blight in the Ruins", Environment.NewLine)
			});
			difficultyLevelSettings.Add(new DifficultyLevelSettings()
			{
				DifficultyLevel = DifficultyLevel.Squire,
				StartingBlights = 1,
				StartingDarkness = 0,
				Notes = string.Format("+1 to time limits{0}Start with 1 spark each", Environment.NewLine)
			});
			difficultyLevelSettings.Add(new DifficultyLevelSettings()
			{
				DifficultyLevel = DifficultyLevel.Adventurer,
				StartingBlights = 1,
				StartingDarkness = 0
			});
			difficultyLevelSettings.Add(new DifficultyLevelSettings()
			{
				DifficultyLevel = DifficultyLevel.Champion,
				StartingBlights = 1,
				StartingDarkness = 5,
				PallOfSuffering = true,
				Notes = "Start with 1 quest in the Village"
			});
			difficultyLevelSettings.Add(new DifficultyLevelSettings()
			{
				DifficultyLevel = DifficultyLevel.Heroic,
				StartingBlights = 2,
				StartingDarkness = 0,
				PallOfSuffering = true,
				Notes = "Start with 1 quest in the Village"
			});
			difficultyLevelSettings.Add(new DifficultyLevelSettings()
			{
				DifficultyLevel = DifficultyLevel.Legendary,
				StartingBlights = 1,
				StartingDarkness = 5,
				PallOfSuffering = true,
				SpawnExtraQuests = true,
				Notes = "Start with 2 quests in random locations"
			});

			difficultyLevelSettings.Add(new DifficultyLevelSettings()
			{
				DifficultyLevel = DifficultyLevel.Custom,
				StartingBlights = _settings.StartingBlights,
				StartingDarkness = _settings.StartingDarkness,
				//PallOfSuffering = _settings.PallOfSuffering
			});

			return difficultyLevelSettings;
		}

	}
}

