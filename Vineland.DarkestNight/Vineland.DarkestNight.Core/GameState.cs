using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Vineland.DarkestNight.Core
{
    public class GameState
    {
        public GameState()
        {
            Heroes = new HeroesState();
            Necromancer = new NecomancerState();

            //TODO: this shoudln't be in here
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Vineland.DarkestNight.Core.locations.json"))
            using (var reader = new StreamReader(stream))
            {
                Locations = JsonConvert.DeserializeObject<List<Location>>(reader.ReadToEnd());
            }
        }

        public string Name { get; set; }
        public DateTime StartedDate { get; set; }

        public List<Location> Locations { get; private set; }
        public HeroesState Heroes { get; private set; }
        public NecomancerState Necromancer { get; private set; }

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
            Active = new List<Hero>();
            FallenHeroIds = new List<int>();        
        }

        public List<Hero> Active { get; set; }
        public List<int> FallenHeroIds { get; set; }

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
