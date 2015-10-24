using System;
using System.Collections.Generic;

namespace Vineland.DarkestNight.Core
{
    public class GameState
    {
        public GameState()
        {
            Heroes = new List<Hero>();
            FallenHeroes = new List<Hero>();
        }

        public string Name { get; set; }

        public List<Hero> Heroes { get; set; }
        public List<Hero> FallenHeroes { get; set; }

        public Location NecromancerLocation { get; set; }
        public int DarknessLevel { get; set; }

        #region Game Modifiers
        public bool PallOfSuffering { get; set; }
        public DarknessCardsMode Mode { get; set; }
        public DarknessCardsState DarknessCards { get; set; }
        public bool DarknessTrackEffectsActive
        {
            get
            {
                return Mode == DarknessCardsMode.None || Mode == DarknessCardsMode.Midnight;
            }
        }
        #endregion
        //TODO Game modifiers (gate, desecreation?)
        //TODO Hero modifiers

        //TODO Necromancer modifiers
    }

    public class DarknessCardsState
    {
        public bool FocusedRituals { get; set; }
        public bool EncroachingShadows { get; set; }
        public bool DyingLand { get; set; }
        public bool Overwhelm { get; set; }
        public bool CreepingShadows { get; set; }
    }

    public enum DarknessCardsMode
    {
        None,
        Standard,
        Twilight,
        Midnight
    }
}
