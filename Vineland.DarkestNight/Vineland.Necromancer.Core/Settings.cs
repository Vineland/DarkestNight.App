using System;
using System.Collections.Generic;
using System.Text;

namespace Vineland.Necromancer.Core
{
    public class Settings
    {
		private const string EXPANSIONS = "Expansions";


		private const string WITH_AN_INNER_LIGHT = "WithAnInnerLight";
		private const string ON_SHIFTING_WINDS = "OnShiftingWinds";
		private const string FROM_THE_ABYSS = "FromTheAbyss";
		private const string IN_TALES_OF_OLD = "InTalesOfOld";
		private const string NYMPH_PROMO = "NymphPromo";
		private const string ENCHANTER_PROMO = "EnchanterPromo";
		private const string MERCENARY_PROMO = "MercenaryPromo";
		private const string TINKER_PROMO = "TinkerPromo";

        ISettingsService _settingsService;

        public Settings(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

		private const string NUMBER_OF_PLAYERS = "NumberOfPlayers";
		private const string DIFFICULTY_LEVEL = "DifficultyLevel";

		public int NumberOfPlayers
		{
			get { return _settingsService.LoadInt(NUMBER_OF_PLAYERS, 4); }
			set { _settingsService.SaveInt(NUMBER_OF_PLAYERS, value); }
		}
		public DifficultyLevel DifficultyLevel
		{
			get { return (DifficultyLevel)_settingsService.LoadInt(DIFFICULTY_LEVEL, (int)DifficultyLevel.Adventurer); }
			set { _settingsService.SaveInt(DIFFICULTY_LEVEL, (int)value); }
		}

		#region Custom Difficulty Settings
		private const string DARKNESS_CARDS_MODE = "DarknessCardsMode";
		private const string PALL_OF_SUFFERING = "PallOfSuffering";
		private const string SPAWN_EXTRA_QUESTS = "SpawnExtraQuests";
		private const string STARTING_DARKNESS = "StartingDarkness";
		private const string STARTING_BLIGHTS_PER_LOCATION = "StartingBlightsPerLocation";

		public int StartingDarkness
		{
			get { return _settingsService.LoadInt(STARTING_DARKNESS); }
			set { _settingsService.SaveInt(STARTING_DARKNESS, value); }
		}

        public bool PallOfSuffering
        {
            get { return _settingsService.LoadBoolean(PALL_OF_SUFFERING); }
            set { _settingsService.SaveBoolean(PALL_OF_SUFFERING, value); }
        }
		public bool SpawnExtraQuests{
			get { return _settingsService.LoadBoolean(SPAWN_EXTRA_QUESTS); }
			set { _settingsService.SaveBoolean(SPAWN_EXTRA_QUESTS, value); }
		}
        public DarknessCardsMode DarknessCardsMode
        {
            get { return (DarknessCardsMode)_settingsService.LoadInt(DARKNESS_CARDS_MODE); }
            set { _settingsService.SaveInt(DARKNESS_CARDS_MODE, (int)value); }
        }
		public int StartingBlights
		{
			get { return _settingsService.LoadInt(STARTING_BLIGHTS_PER_LOCATION); }
			set { _settingsService.SaveInt(STARTING_BLIGHTS_PER_LOCATION, value); }
		}

		#endregion
		public int Expansions{
			get { return _settingsService.LoadInt (EXPANSIONS); }
			set{ _settingsService.SaveInt (EXPANSIONS, value); }
		}

		public bool WithAnInnerLight
		{
			get { return _settingsService.LoadBoolean(WITH_AN_INNER_LIGHT, true); }
			set 
			{ 
				_settingsService.SaveBoolean(WITH_AN_INNER_LIGHT, value); 
				if (!value)
					PallOfSuffering = false;
			}
		}

		public bool OnShiftingWinds
		{
			get { return _settingsService.LoadBoolean(ON_SHIFTING_WINDS, true); }
			set { _settingsService.SaveBoolean(ON_SHIFTING_WINDS, value); }
		}

		public bool FromTheAbyss
		{
			get { return _settingsService.LoadBoolean(FROM_THE_ABYSS, true); }
			set 
			{ 
				_settingsService.SaveBoolean(FROM_THE_ABYSS, value); 
				if (!value)
					DarknessCardsMode = DarknessCardsMode.None;
			}
		}

		public bool InTalesOfOld
		{
			get { return _settingsService.LoadBoolean(IN_TALES_OF_OLD, true); }
			set { _settingsService.SaveBoolean(IN_TALES_OF_OLD, value); }
		}

		public bool NymphPromo
		{
			get { return _settingsService.LoadBoolean(NYMPH_PROMO, true); }
			set { _settingsService.SaveBoolean(NYMPH_PROMO, value); }
		}

		public bool EnchanterPromo
		{
			get { return _settingsService.LoadBoolean(ENCHANTER_PROMO, true); }
			set { _settingsService.SaveBoolean(ENCHANTER_PROMO, value); }
		}

		public bool MercenaryPromo
		{
			get { return _settingsService.LoadBoolean(MERCENARY_PROMO, true); }
			set { _settingsService.SaveBoolean(MERCENARY_PROMO, value); }
		}

		public bool TinkerPromo
		{
			get { return _settingsService.LoadBoolean(TINKER_PROMO, true); }
			set { _settingsService.SaveBoolean(TINKER_PROMO, value); }
		}
    }
}
