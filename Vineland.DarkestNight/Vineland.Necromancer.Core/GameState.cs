using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.Core
{
	public class GameState
	{
		public GameState ()
		{
			Necromancer = new NecomancerState ();
			Heroes = new HeroesState ();
		}

		public List<Location> Locations { get; set; }

		public Deck<MapCard> MapCards { get; set; }

		public List<Blight> BlightPool { get; set; }

		public HeroesState Heroes { get; protected set; }

		public NecomancerState Necromancer { get; protected set; }

		public int Darkness { get; set; }

		public int NumberOfPlayers {get;set;}

		//public DifficultyLevel DifficultyLevel {get;set;}

		public bool UseQuests { get; set; }

		//public bool SpawnExtraQuests { get; set; }
		public DarknessCardsMode Mode { get; set; }

		public bool DarknessTrackEffectsActive {
			get {
				//return true;
				return Mode == DarknessCardsMode.None || Mode == DarknessCardsMode.Midnight;
			}
		}

		public bool GatesActive{
			get{ return Locations.Any (l => l.Blights.Any (b => b.Name == "Gate")); }
		}


		public GameState Clone(){
			var json = JsonConvert.SerializeObject (this, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
			return JsonConvert.DeserializeObject<GameState> (json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
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
	}

	public class HeroesState{
		public List<Hero> Active {get;set;}

		#region Artifacts
		public int? VoidArmorHeroId { get; set; }
		public bool ShieldOfRadianceActive { get; set; }
		public bool BlindingBlackActive {get;set;}
		public int? InvisibleBarrierLocationId { get; set; }
		public bool AuraOfHumilityActive {get;set;}
		public bool HermitActive { get; set; }
		public int? AncientDefenseLocationId {get;set;}
		public int ProphecyOfDoomRoll {get;set;}
		public bool ElusiveSpiritActive {get;set;}
		public bool DecoyActive { get; set; }
		public bool RuneOfMisdirectionActive { get; set; }
		//public bool RuneOfClairvoyanceActive {get;set;}
		//public bool SeeingGlassActive { get; set; }
		#endregion
	}

	public enum DarknessCardsMode
	{
		None,
		Standard,
		Twilight,
		Midnight
	}
}
