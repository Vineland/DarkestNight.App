using System;
using System.Collections.Generic;
using System.Text;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.Core
{
    public class Settings
    {

        ISettingsService _settingsService;

        public Settings(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

		#region General
		private const string NUMBER_OF_PLAYERS = "NumberOfPlayers";
		private const string DIFFICULTY_LEVEL = "DifficultyLevel";

		public int NumberOfPlayers
		{
			get { return _settingsService.LoadInt(NUMBER_OF_PLAYERS, 4); }
			set { _settingsService.SaveInt(NUMBER_OF_PLAYERS, value); }
		}
		public DifficultyLevelId DifficultyLevel
		{
			get { return (DifficultyLevelId)_settingsService.LoadInt(DIFFICULTY_LEVEL, (int)DifficultyLevelId.Adventurer); }
			set { _settingsService.SaveInt(DIFFICULTY_LEVEL, (int)value); }
		}

		#endregion
    }
		
}
