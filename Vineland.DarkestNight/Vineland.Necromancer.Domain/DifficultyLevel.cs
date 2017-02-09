using System;

namespace Vineland.Necromancer.Domain
{
	public class DifficultyLevel
	{
		public DifficultyLevelId Id { get; set; }
		public string Name { get; set;}
		public int StartingDarkness { get; set; }
		public int StartingBlights {get;set;}
		public bool SpawnExtraQuests {get;set;}
		public string Notes {get;set;}
	}

	public enum DifficultyLevelId
	{
		Page = 1,
		Squire,
		Adventurer,
		Champion,
		Heroic,
		Legendary
	}
}

