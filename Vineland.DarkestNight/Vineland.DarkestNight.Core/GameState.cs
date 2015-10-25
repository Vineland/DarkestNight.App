using System;
using System.Collections.Generic;

namespace Vineland.DarkestNight.Core
{
    public class GameState
    {
        public GameState()
        {
            Heroes = new HeroesState();
        }

        public string Name { get; set; }
        
        public HeroesState Heroes { get; set; }

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

    public class HeroesState {

        public HeroesState()
        {
            Active = new List<Hero>();
            Fallen = new List<Hero>();        
        }

        public List<Hero> Active { get; set; }
        public List<Hero> Fallen { get; set; }

        #region Effects
        public bool HermitActive { get; set; }
        public int? AuraOfHumilityLocationId { get; set; }
        #endregion
    }

    public class NecomancerState
    {
        public int LocationId { get; set; }
        
        public bool FocusedRituals { get; set; }
        public bool EncroachingShadows { get; set; }
        public bool DyingLand { get; set; }
        public bool Overwhelm { get; set; }
        public bool CreepingShadows
        {
            get; set;
        }
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
