using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Vineland.Necromancer.Core
{
	public class GameState
	{
		public GameState ()
		{
			Heroes = new HeroesState ();
			Necromancer = new NecomancerState ();
		}


		public DateTime CreatedDate { get; set; }

		public List<Location> Locations { get; set; }

		public HeroesState Heroes { get; protected set; }

		public NecomancerState Necromancer { get; protected set; }

		public int Darkness { get; set; }

		public bool PallOfSuffering { get; set; }

		public DarknessCardsMode Mode { get; set; }

		public bool DarknessTrackEffectsActive {
			get {
				return Mode == DarknessCardsMode.None || Mode == DarknessCardsMode.Midnight;
			}
		}

	}

	public class HeroesState
	{

		public HeroesState ()
		{
			Active = new List<Hero> ();
		}

		public List<Hero> Active { get; set; }

		#region Effects

		public bool HermitActive { get; set; }

		public bool AuraOfHumilityActive { get; set; }

		public bool RuneOfMisdirectionActive { get; set; }

		public bool BlindingBlackActive { get; set; }

		public bool ElusiveSpiritActive { get; set; }

		public bool DecoyActive { get; set; }

		public int ProphecyOfDoomRoll { get; set; }

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
