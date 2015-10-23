using System;
using System.Collections.Generic;

namespace Vineland.DarkestNight.Core.Model
{
    public class GameState
    {
        public GameState()
        {

        }

        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }

        public List<Hero> Heroes { get; set; }
        public int DarknessLevel { get; set; }


        public bool PallOfSuffering { get; set; }
        public DarknessCardsMode DarknessCardsMode { get; set; }
        //TODO Game modifiers
        //TODO Hero modifiers
        //TODO Necromancer modifiers

    }

    public enum DarknessCardsMode {
        None,
        Standard,
        Twilight,
        Midnight
    }
}
