using System.Collections.Generic;
using System.Linq;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.Core.Services;
using Vineland.Necromancer.Domain;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class NecromancerDetectionPageModel :BaseViewModel
	{
		NecromancerService _necromancerService;

		public NecromancerDetectionPageModel (NecromancerService necromancerService)
		{
			_necromancerService = necromancerService;

			Results = new List<NecromancerDetectionResultViewModel> ();

			var seer = Application.CurrentGame.Heroes.GetHero<Seer>();
			var wizard = Application.CurrentGame.Heroes.GetHero<Wizard>();

			if (seer == null || seer.ProphecyOfDoomRoll == 0)
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService));
			else
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService, seer.ProphecyOfDoomRoll));

			if (wizard != null && wizard.RuneOfMisdirectionActive)
				Results.Add (new NecromancerDetectionResultViewModel (_necromancerService));

			SelectedResult = Results.First ();
		}

		public List<NecromancerDetectionResultViewModel> Results { get; set; }

		public NecromancerDetectionResultViewModel SelectedResult { get; set; }

		public bool SkipPage {
			get {
				return Results.Count == 1 && !Results.Any (r => r.DetectedHero != null && (r.BlindingBlackVisible || r.DecoyVisible || r.ElusiveSpiritVisible || r.VoidArmourVisible));
			}
		}

		public Command AcceptCommand {
			get {
				return new Command (async (obj) => {
					await CoreMethods.PushPageModel<NecromancerActivationPageModel> ();
					//(page.BindingContext as NecromancerActivationViewModel).Initialise (SelectedResult.DetectedHero, SelectedResult.NecromancerRoll, SelectedResult.HeroesToIgnore.ToArray());
				});
			}
		}
	}

	public class NecromancerDetectionResultViewModel : BaseViewModel
	{
		NecromancerService _necromancerService;

		public NecromancerDetectionResultViewModel (NecromancerService necromancerService, int? roll = null)
		{
			_necromancerService = necromancerService;
			if (!roll.HasValue)
				roll = new D6GeneratorService ().RollDemBones ();

			NecromancerRoll = roll.Value;
			DetectHero ();
		}

		public int NecromancerRoll { get; set; }

		private int DetectionRoll {
			get { return NecromancerRoll + (Application.CurrentGame.GatesActive ? 1 : 0); }
		}

		public Hero DetectedHero { get; set; }

		public string NecromancerRollDisplay {
			get { 
				var display = NecromancerRoll.ToString ();
				if (Application.CurrentGame.GatesActive)
					display += " (+1 for detection)";

				return display;
			}
		}

		public List<int> HeroesToIgnore { get; set; } = new List<int> ();

		public bool VoidArmourVisible {
			get
			{ 
				return Application.CurrentGame.Heroes.Any(h => h.HasVoidArmour && h.Id == DetectedHero.Id);
			}
		}

		public Command VoidArmourCommand {
			get {
				return new Command (() => {
					HeroesToIgnore.Add (DetectedHero.Id);
					DetectHero ();
				},
					() => {
						return DetectedHero != null;
					});
			}
		}

		public bool BlindingBlackVisible {
			get {
				var acolyte = Application.CurrentGame.Heroes.GetHero<Acolyte>();
				return acolyte != null && acolyte.BlindingBlackActive;
			}
		}

		public Command BlindingBlackCommand {
			get {
				return new Command (() => {
					HeroesToIgnore = Application.CurrentGame.Heroes.Select (x => x.Id).ToList ();
					//_acolyte.BlindingBlackActive = false;
					DetectHero ();
				},
					() => {
						return DetectedHero != null
						&& Application.CurrentGame.Heroes.GetHero<Acolyte>().BlindingBlackActive;
					});
			}
		}

		public bool DecoyVisible {
			get {
				var wayfarer = Application.CurrentGame.Heroes.GetHero<Wayfarer>();
				return wayfarer != null && wayfarer.DecoyActive;
			}
		}

		public Command DecoyCommand {
			get {
				return new Command (() => {
					DetectedHero = Application.CurrentGame.Heroes.GetHero<Wayfarer>();
					RaisePropertyChanged (() => DetectedHero);
					RaisePropertyChanged (() => DecoyCommand);
					RaisePropertyChanged (() => VoidArmourCommand);
				},
					() => {
						var wayfarer = Application.CurrentGame.Heroes.GetHero<Wayfarer>();
						return DetectedHero != null
							&& wayfarer.LocationId != LocationId.Monastery
							&& wayfarer.Secrecy >= DetectionRoll;
					});
			}
		}

		public bool ElusiveSpiritVisible {
			get {
				var valkyrie = Application.CurrentGame.Heroes.GetHero<Valkyrie>();

				return valkyrie != null && valkyrie.ElusiveSpiritActive;
			}
		}

		public Command ElusiveSpiritCommand {
			get {
				return new Command (() => {
					HeroesToIgnore.Add (DetectedHero.Id);
					DetectHero ();
				},
					() => {
						return DetectedHero != null
						&& DetectionRoll == DetectedHero.Secrecy + 1;
					});
			}
		}

		public void DetectHero ()
		{
			DetectedHero = _necromancerService.Detect (Application.CurrentGame, NecromancerRoll, HeroesToIgnore.ToArray ());
			RaisePropertyChanged (() => DetectedHero);
			RaisePropertyChanged (() => ElusiveSpiritCommand);
			RaisePropertyChanged (() => BlindingBlackCommand);
			RaisePropertyChanged (() => DecoyCommand);
			RaisePropertyChanged (() => VoidArmourCommand);
		}
	}
}

