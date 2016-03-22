﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vineland.Necromancer.Core.Services;
using System.Globalization;
using System.Xml.Linq;
using System.Threading;
using Newtonsoft.Json.Serialization;

namespace Vineland.Necromancer.Core
{
	public class NecromancerService
	{
		D6GeneratorService _d6GeneratorService;
		BlightService _blightService;

		public NecromancerService (D6GeneratorService d6GeneratorService, BlightService blightService)
		{
			_d6GeneratorService = d6GeneratorService;
			_blightService = blightService;
		}

		/// <summary>
		/// Activate the necromancer. First he detects and then moves.
		/// </summary>
		/// <param name="gameState">Game state.</param>
		/// <param name="detectedHero">Force this hero to be detected.</param>
		/// <param name="roll">Force the necromancer roll.</param>
		/// <param name="heroesToIgnore">Heroes that are ignored due to Elusive Spirit or Blinding Black</param>
		public NecromancerDetectionResult Activate (GameState gameState, 
		                                             Hero detectedHero = null,
		                                             int? roll = null, 
		                                             int[] heroesToIgnore = null)
		{
			var result = new NecromancerDetectionResult ();
			result.NecromancerRoll = roll.HasValue ? roll.Value : _d6GeneratorService.RollDemBones ();
			result.DetectionRoll = gameState.GatesActive ? result.NecromancerRoll + 1 : result.NecromancerRoll;
			result.OldLocationId = gameState.Necromancer.LocationId;

			result.DarknessIncrease = 1 + gameState.Locations.SelectMany (x => x.Blights).Count (x => x.Name == "Desecration");
			if (gameState.Heroes.Any (x => x.HasShieldOfRadiance) && result.NecromancerRoll == 6) {
				result.DarknessIncrease--;
				result.Notes += "Shield of Radiance triggered" + Environment.NewLine;
			}

			gameState.Darkness += result.DarknessIncrease;
				
			//detect
			if (detectedHero == null)
				Detect (gameState, result, heroesToIgnore);
			else
				result.DetectedHero = detectedHero;

			//move
			Move (gameState, result);
			
			return result;
		}

		private void Detect (GameState gameState, NecromancerDetectionResult result,
		                     int[] heroesToIgnore)
		{
			//first check if any heroes can be detected
			var exposedHeroes = gameState.Heroes.Where (x => x.LocationId != (int)LocationIds.Monastery && x.Secrecy < result.DetectionRoll && (heroesToIgnore == null || !heroesToIgnore.Contains (x.Id)));

			//special hero effects
			var ranger = gameState.Heroes.SingleOrDefault (x => x is Ranger) as Ranger;
			if (ranger != null && ranger.HermitActive && ranger.LocationId == (int)LocationIds.Swamp)
				exposedHeroes = exposedHeroes.Where (h => h != ranger);

			var paragon = gameState.Heroes.SingleOrDefault (h => h is Paragon) as Paragon;
			if (paragon != null && paragon.AuraOfHumilityActive)
				exposedHeroes = exposedHeroes.Where (x => x.LocationId != paragon.LocationId);

			if (exposedHeroes.Any ()) {
				//figure out which hero the necromancer is pursuing

				if (gameState.GatesActive) { 
					//if gates are active pick a random hero from those detectd
					var index = new Random ().Next (0, exposedHeroes.Count () - 1);
					result.DetectedHero = exposedHeroes.ToArray () [index];
				} else if (exposedHeroes.Any (x => x.LocationId == gameState.Necromancer.LocationId)) {
					result.DetectedHero = exposedHeroes.First (x => x.LocationId == gameState.Necromancer.LocationId);
				} else {
					//TODO: this needs work. not exactly readable at the moment
					var necromancerLocation = gameState.Locations.First (x => x.Id == gameState.Necromancer.LocationId);

					var heroesOneLocationAway = exposedHeroes.Where (x => necromancerLocation.Pathways.Any (y => y == x.LocationId));
					if (heroesOneLocationAway.Any ())
						exposedHeroes = heroesOneLocationAway;

					var index = new Random ().Next (0, exposedHeroes.Count () - 1);
					result.DetectedHero = exposedHeroes.ToArray () [index];
				}
			} else
				result.DetectedHero = null;
		}

		/// <summary>
		/// Moves the necromancer to a new location and updates the result.
		/// </summary>
		private void Move (GameState gameState, NecromancerDetectionResult result)
		{
			var currentLocation = gameState.Locations.Single (x => x.Id == gameState.Necromancer.LocationId);
			Location newLocation = null;

			if (result.DetectedHero != null) {
				var heroLocation = gameState.Locations.Single (x => x.Id == result.DetectedHero.LocationId);
				var necromancerLocation = gameState.Locations.First (x => x.Id == gameState.Necromancer.LocationId);
				//if the necromancer can reach the hero go directly to them
				if (necromancerLocation.Pathways.Contains (heroLocation.Id) || gameState.GatesActive)
					newLocation = heroLocation;
				else {
					//the common locations between the hero and necromancer are the ones that the necro should move along
					var commonLocationIds = necromancerLocation.Pathways.Intersect (heroLocation.Pathways).ToArray ();
					var index = new Random ().Next (0, commonLocationIds.Length - 1);
					newLocation = gameState.Locations.Single (x => x.Id == commonLocationIds [index]);
				}
			} else
				newLocation = gameState.Locations.Single (x => x.Id == currentLocation.Pathways [result.NecromancerRoll - 1]);

			var conjurer = gameState.Heroes.GetHero<Conjurer> ();
			if (conjurer != null
			   && conjurer.InvisibleBarrierLocationId.HasValue
			   && conjurer.InvisibleBarrierLocationId.Value == newLocation.Id) 
			{
				newLocation = currentLocation;
				conjurer.InvisibleBarrierLocationId = null;
				result.Notes += string.Format ("Invisible Barrier deactivated and cancelled the Necromancer's movement. {0}", Environment.NewLine);
			}

			gameState.Necromancer.LocationId = newLocation.Id;
			result.NewLocationId = newLocation.Id;
		}

		public NecromancerSpawnResult Spawn (GameState gameState,NecromancerDetectionResult detectionResult)
		{
			var notes = new StringBuilder ();
			var result = new NecromancerSpawnResult (){ NewBlights = new List<Tuple<Location, Blight>> () };

			var newLocation = gameState.Locations.Single (l => l.Id == detectionResult.NewLocationId);

			//check if a quest needs to be spawned
			if (gameState.PallOfSuffering && (detectionResult.NecromancerRoll == 3 || detectionResult.NecromancerRoll == 4))
				result.SpawnQuest = true;

			var initialBlightCount = newLocation.BlightCount;
			var monastery = gameState.Locations.Single (l => l.Id == (int)LocationIds.Monastery);

			//spawn blight at necromancer's new location
			result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));		

			//standard darkness track effects
			if (gameState.DarknessTrackEffectsActive) {
				if (gameState.Darkness >= 10 && newLocation.BlightCount == 0)
					result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));

				if (gameState.Darkness >= 20 && (detectionResult.NecromancerRoll == 1 || detectionResult.NecromancerRoll == 2))
					result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));
			}

			//darkness card effects
			if (gameState.Mode != DarknessCardsMode.None) {
				if (gameState.Necromancer.FocusedRituals
					&& !gameState.Heroes.Any (x => x.LocationId == newLocation.Id))
					result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));

				if (gameState.Necromancer.CreepingShadows && (detectionResult.NecromancerRoll == 5 || detectionResult.NecromancerRoll == 6))
					result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));

				if (gameState.Necromancer.DyingLand) {
					if (gameState.DarknessTrackEffectsActive
					    && gameState.Darkness >= 10
						&& newLocation.BlightCount == 1)
						result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));
					else if ((!gameState.DarknessTrackEffectsActive
					         || gameState.Darkness < 10)
						&& newLocation.BlightCount == 0)
						result.NewBlights.Add (_blightService.SpawnBlight (newLocation, gameState));
				}

				if (gameState.Necromancer.EncroachingShadows && detectionResult.NecromancerRoll == 6)
					result.NewBlights.Add (_blightService.SpawnBlight (monastery, gameState));

				if (gameState.Necromancer.Overwhelm && initialBlightCount < 4 && newLocation.BlightCount >= 4)
					result.NewBlights.Add (_blightService.SpawnBlight (monastery, gameState));

			}

			//darkness tipping over 30
			if (gameState.Darkness > 30) {
				var overflowCount = gameState.Darkness - 30;

				for (int i = 0; i < overflowCount; i++)
					result.NewBlights.Add (_blightService.SpawnBlight (monastery, gameState));

				gameState.Darkness = 30;
			}
			return result;
		}
	}

	public class NecromancerDetectionResult
	{
		public Hero DetectedHero { get; set; }

		public int DarknessIncrease {get;set;}

		public int NecromancerRoll { get; set; }

		public int DetectionRoll { get; set; }

		public int OldLocationId { get; set; }

		public int NewLocationId { get; set; }

		public string Notes{ get; set; }
	}

	public class NecromancerSpawnResult
	{
		public List<Tuple<Location, Blight>> NewBlights { get; set; }

		public bool SpawnQuest { get; set; }

		public string Notes { get; set; }
	}
}
