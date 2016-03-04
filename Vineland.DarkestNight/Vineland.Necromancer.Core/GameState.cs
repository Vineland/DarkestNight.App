using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using System.Dynamic;

namespace Vineland.Necromancer.Core
{
	public class GameState
	{
		public GameState ()
		{
			Necromancer = new NecomancerState ();
		}

		public List<Location> Locations { get; set; }

		public Deck<MapCard> MapCards { get; set; }

		public List<Blight> BlightPool { get; set; }

		public List<Hero> Heroes { get; set; }

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
