using System;
using System.Collections.Generic;
using System.Text;

namespace Vineland.Necromancer.Core
{
    public class Settings
    {

        ISettingsService _settingsService;

        public Settings(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

		public Editions Edition
		{
			get { return (Editions)_settingsService.LoadInt(EDITION, (int)Editions.First); }
			set { _settingsService.SaveInt(EDITION, (int)value); }
		}

		#region General
		private const string EDITION = "Edition";
		//private const string NUMBER_OF_PLAYERS = "NumberOfPlayers";
		//private const string DIFFICULTY_LEVEL = "DifficultyLevel";
		private const string STARTING_DARKNESS = "StartingDarkness";
		private const string STARTING_BLIGHTS_PER_LOCATION = "StartingBlightsPerLocation";

		//public int NumberOfPlayers
		//{
		//	get { return _settingsService.LoadInt(NUMBER_OF_PLAYERS, 4); }
		//	set { _settingsService.SaveInt(NUMBER_OF_PLAYERS, value); }
		//}
		//public DifficultyLevel DifficultyLevel
		//{
		//	get { return (DifficultyLevel)_settingsService.LoadInt(DIFFICULTY_LEVEL, (int)DifficultyLevel.Adventurer); }
		//	set { _settingsService.SaveInt(DIFFICULTY_LEVEL, (int)value); }
		//}
		public int StartingDarkness
		{
			get { return _settingsService.LoadInt(STARTING_DARKNESS); }
			set { _settingsService.SaveInt(STARTING_DARKNESS, value); }
		}

		public int StartingBlights
		{
			get { return _settingsService.LoadInt(STARTING_BLIGHTS_PER_LOCATION); }
			set { _settingsService.SaveInt(STARTING_BLIGHTS_PER_LOCATION, value); }
		}
		#endregion

		#region First Edition

		private const string EXPANSIONS = "Expansions";
		private const string DARKNESS_CARDS_MODE = "DarknessCardsMode";
		private const string USE_QUESTS = "UseQuests";

		public Expansion Expansions{
			get { return (Expansion)_settingsService.LoadInt (EXPANSIONS); }
			set{ _settingsService.SaveInt (EXPANSIONS, (int)value); }
		}
		public bool UseQuests
		{
			get { return _settingsService.LoadBoolean(USE_QUESTS); }
			set { _settingsService.SaveBoolean(USE_QUESTS, value); }
		}
		public DarknessCardsMode DarknessCardsMode
		{
			get { return (DarknessCardsMode)_settingsService.LoadInt(DARKNESS_CARDS_MODE); }
			set { _settingsService.SaveInt(DARKNESS_CARDS_MODE, (int)value); }
		}

		#endregion
    }
		
}
