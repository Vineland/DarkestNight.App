using System;

namespace Vineland.Necromancer.Core
{
	public class DifficultyLevelSettings
	{
		public DifficultyLevel DifficultyLevel { get; set;}
		public int StartingDarkness { get; set; }
		public int StartingBlights {get;set;}
		public bool PallOfSuffering {get;set;}
		public bool SpawnExtraQuests {get;set;}
		public string Notes {get;set;}
	}

	public enum DifficultyLevel{
		Page,
		Squire,
		Adventurer,
		Champion,
		Heroic,
		Legendary,
		Custom
	}
}

