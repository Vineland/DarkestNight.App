using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Android.Util;
using System.Linq;

namespace Vineland.DarkestNight.Core
{
    public class GameState
    {
        public GameState()
        {
            Heroes = new HeroesState();
            Necromancer = new NecomancerState();
			Locations = Location.All;
        }


        public DateTime CreatedDate { get; set; }

        public List<Location> Locations { get; protected set; }
        public HeroesState Heroes { get; protected set; }
        public NecomancerState Necromancer { get; protected set; }

        public int DarknessLevel { get; set; }
        public bool PallOfSuffering { get; set; }
        public DarknessCardsMode Mode { get; set; }
        public bool DarknessTrackEffectsActive
        {
            get
            {
                return Mode == DarknessCardsMode.None || Mode == DarknessCardsMode.Midnight;
            }
        }

	}

    public class HeroesState {

        public HeroesState()
        {
			All = Hero.All;
        }

		public List<Hero> All { get; private set; }
		public IEnumerable<Hero> Active { get { return All.Where (x => x.IsActive); } }
		public IEnumerable<Hero> Fallen { get { return All.Where (x => x.HasFallen); } }

        #region Effects
        public bool HermitActive { get; set; }
        public int? AuraOfHumilityLocationId { get; set; }
        #endregion
    }

    public class NecomancerState
    {
        public int LocationId { get; set; }

        #region Darkness Cards
        public bool FocusedRituals { get; set; }
        public bool EncroachingShadows { get; set; }
        public bool DyingLand { get; set; }
        public bool Overwhelm { get; set; }
        public bool CreepingShadows { get; set; }
        #endregion

        public bool GatesActive { get; set; }
    }

    public enum DarknessCardsMode
    {
        None,
        Standard,
        Twilight,
        Midnight
    }
}
