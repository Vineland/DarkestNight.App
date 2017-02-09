using System;
using System.Collections.Generic;
using System.Linq;
using Vineland.Necromancer.Domain;
using Vineland.Necromancer.Repository;

namespace Vineland.Necromancer.Core
{
	public class DataService
	{	
		IRepository _repository;

		public DataService(IRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<Blight> GetBlights()
		{
			return _repository.GetAll<Blight>().OrderBy(x => x.Name).ToList();
		}

		public IEnumerable<MapCard> GetMapCards()
		{
			var mapCards = _repository.GetAll<MapCard>().ToList();

			mapCards.Shuffle();

			return mapCards;
		}

		public IEnumerable<Hero> GetAllHeroes()
		{
			var heroes = _repository.GetAll<Hero>();

			return heroes.OrderBy(x => x.Name).ToList();
		}


		public IEnumerable<Location> GetLocations()
		{
			return _repository.GetAll<Location>();
		}

		public IEnumerable<DifficultyLevel> GetDifficultyLevelSettings()
		{
			return _repository.GetAll<DifficultyLevel>();
		}

	}
}

