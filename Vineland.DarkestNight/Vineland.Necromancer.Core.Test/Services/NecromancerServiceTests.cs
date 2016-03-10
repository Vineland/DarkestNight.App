using System;
using NUnit.Framework;
using Moq;
using Vineland.Necromancer.Core.Services;
using System.Linq;
using System.Collections.Generic;

namespace Vineland.Necromancer.Core.Test
{
	[TestFixture]
	public class NecromancerServiceTests
	{
		NecromancerService _necromancerService;
		GameState _gameState;
		Mock<D6GeneratorService> _mockD6Service;
		DataService _dataService;

		[SetUp]
		public void Setup()
		{
			_mockD6Service = new Mock<D6GeneratorService>();
			_mockD6Service.Setup(x => x.RollDemBones()).Returns(6);

			_necromancerService = new NecromancerService(_mockD6Service.Object);
			_dataService = new DataService(null);

			_gameState = new GameState();
			_gameState.Locations = new List<Location> (_dataService.GetLocations());
		}

		[Test]
		public void Detect_HeroSecrecy6Roll6_NoDetections()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 6 });

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.IsNull(result.DetectedHero.Id);
		}

		[Test]
		public void Detect_HeroSecrecy5Roll6_HeroDetected()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId= LocationIds.Castle });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2, LocationId= LocationIds.Castle });

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.AreEqual(1, result.DetectedHero.Id);
		}

		[Test]
		public void Detect_HeroAtMonasterySecrecy5Roll6_NotDetected()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Monastery });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2 });

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.IsNull(result.DetectedHero.Id);
		}

		[Test]
		public void Detect_MultipleHeroesDetected_HeroAtCurrentLocationChosen()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Castle });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2, LocationId = LocationIds.Swamp });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3, LocationId = LocationIds.Ruins });
			_gameState.Necromancer.LocationId = LocationIds.Ruins;

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.AreEqual(3, result.DetectedHero.Id);
		}

		[Test]
		public void Detect_MultipleHeroesDetected_ClosestHeroChosen()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Castle });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 2, LocationId = LocationIds.Swamp });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 3, LocationId = LocationIds.Ruins });
			_gameState.Necromancer.LocationId = LocationIds.Ruins;

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.AreEqual(2, result.DetectedHero.Id);
		}

		[Test]
		public void Detect_MultipleHeroesDetected_RandomHeroOneDistanceAwayChosen()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Forest });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 2, LocationId = LocationIds.Swamp });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3, LocationId = LocationIds.Mountains });
			_gameState.Necromancer.LocationId = LocationIds.Ruins;

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.AreNotEqual(3, result.DetectedHero.Id);
		}

		[Test]
		public void Detect_MultipleHeroesDetected_RandomHeroDistanceTwoAwayChosen()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Swamp });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 2, LocationId = LocationIds.Castle });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3, LocationId = LocationIds.Monastery });
			_gameState.Necromancer.LocationId = LocationIds.Forest;

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.AreNotEqual(3, result.DetectedHero.Id);
		}

		[Test]
		public void Detect_MultipleHeroesDetectedAndGatesActive_RandomHeroChosen()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Castle });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 2, LocationId = LocationIds.Ruins });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3, LocationId = LocationIds.Forest });
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Necromancer.GatesActive = true;

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.IsTrue(result.DetectedHero.Id == 1 || result.DetectedHero.Id == 2 || result.DetectedHero.Id == 3);
		}

		[Test]
		public void Detect_RangerWithHermitAtSwamp_NotDetected()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, Name = "Ranger", LocationId = LocationIds.Swamp });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2, LocationId= LocationIds.Castle });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3, LocationId= LocationIds.Castle });
			_gameState.Heroes.HermitActive = true;

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.AreEqual(3, result.DetectedHero.Id);
		}

		[Test]
		public void Detect_RangerWithHermitNotSwamp_HeroDetected()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, Name = "Ranger", LocationId = LocationIds.Castle });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2 });
			_gameState.Heroes.HermitActive = true;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.AreEqual(1, result.DetectedHero.Id);
		}

		[Test]
		public void Detect_AuraOfHumilityActiveAtMountains_NoDetections()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 1, Id = 1, LocationId = LocationIds.Mountains });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 1, Id = 2, LocationId = LocationIds.Mountains });
			_gameState.Heroes.Active.Add(new Hero() { Secrecy = 1, Id = 3, Name="Paragon", LocationId = LocationIds.Mountains });
			_gameState.Heroes.AuraOfHumilityActive = true;

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.IsNull(result.DetectedHero.Id);
		}

		[Test]
		public void Detect_HeroSecrecy6Roll6GatesActive_HeroDetected()
		{
			//arrange
			_gameState.Heroes.Active.Add(new Hero() { Id = 1, Secrecy = 6 , LocationId= LocationIds.Castle});
			_gameState.Necromancer.GatesActive = true;

			//act
			var result = _necromancerService.Activate(_gameState);

			//assert
			Assert.AreEqual(1, result.DetectedHero.Id);
		}

		[Test]
		public void Move_NoDetectedHero_FollowsPathway()
		{
			_gameState.Necromancer.LocationId = LocationIds.Mountains;

			var result = _necromancerService.Activate(_gameState, roll: 1);

			Assert.AreEqual(LocationIds.Castle, result);
		}

		[Test]
		public void Move_DetectedHeroOneDistanceAway_MovesToHeroLocation()
		{
			_gameState.Necromancer.LocationId = LocationIds.Mountains;

			var result = _necromancerService.Activate(_gameState, detectedHero: new Hero() { Id = 1, LocationId = LocationIds.Castle }, roll: 2);

			Assert.AreEqual(LocationIds.Castle, result);
		}

		[Test]
		public void Move_DetectedHeroTwoDistanceAway_MovesAlongShortestPath()
		{
			_gameState.Necromancer.LocationId = LocationIds.Ruins;

			var result = _necromancerService.Activate(_gameState, detectedHero: new Hero() { Id = 1, LocationId = LocationIds.Mountains }, roll: 2);

			Assert.IsTrue(result.NewLocationId.Id == LocationIds.Village);
		}

		[Test]
		public void Move_DetectedHeroTwoDistanceAway_MovesAlongOneOfTheShortestPaths()
		{
			_gameState.Necromancer.LocationId = LocationIds.Swamp;

			var result = _necromancerService.Activate(_gameState, detectedHero: new Hero() { Id = 1, LocationId = LocationIds.Mountains }, roll: 2);

			Assert.IsTrue(result.NewLocationId.Id == LocationIds.Village || result.NewLocationId.Id == LocationIds.Castle);
		}

		[Test]
		public void Move_GatesActive_MovesToHeroLocation()
		{
			_gameState.Necromancer.LocationId = LocationIds.Swamp;
			_gameState.Necromancer.GatesActive = true;

			var result = _necromancerService.Activate(_gameState, detectedHero: new Hero() { Id = 1, LocationId = LocationIds.Mountains }, roll: 2);

			Assert.AreEqual(LocationIds.Mountains, result);
		}

		[Test]
		public void Spawn_NoDarknessEffectOrCards_OneNewBlight()
		{
			_gameState.Necromancer.LocationId = LocationIds.Swamp;
	
			var result = _necromancerService.Activate(_gameState, roll:2);

			Assert.AreEqual(1, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_Darkness9LocationEmpty_OneNewBlight()
		{
			_gameState.Darkness = 9;
			_gameState.Mode = DarknessCardsMode.Midnight;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight>();

			var result = _necromancerService.Activate(_gameState, roll: 3);

			Assert.AreEqual(1, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_Darkness10LocationEmpty_TwoNewBlights()
		{
			_gameState.Darkness = 10;
			_gameState.Mode = DarknessCardsMode.Midnight;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight>();

			var result = _necromancerService.Activate(_gameState,roll: 3);

			Assert.AreEqual(2, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_Darkness10LocationNotEmpty_OneNewBlight()
		{
			_gameState.Darkness = 10;
			_gameState.Mode = DarknessCardsMode.Midnight;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight>(){ new Blight()};

			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(1, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_NewLocationHas4Blights_OneNewBlightAtMonastery()
		{
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight> () {
				new Blight(),
				new Blight(),
				new Blight(),
				new Blight()
			};

			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(0, result.NumberOfBlightsToNewLocation);
			Assert.AreEqual(1, result.NumberOfBlightsToMonastery);
		}

		[Test]
		public void Spawn_Darkness19RollOne_OneNewBlight()
		{
			_gameState.Darkness = 19;
			_gameState.Mode = DarknessCardsMode.Midnight;

			var result = _necromancerService.Activate(_gameState, roll:1);

			Assert.AreEqual(1, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_Darkness20RollOne_TwoNewBlights()
		{
			_gameState.Darkness = 20;
			_gameState.Mode = DarknessCardsMode.Midnight;

			var result = _necromancerService.Activate(_gameState, roll:1);

			Assert.AreEqual(2, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_Darkness20Roll3_OneNewBlight()
		{
			_gameState.Darkness = 20;
			_gameState.Mode = DarknessCardsMode.Midnight;

			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(1, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_PallOfSufferingRoll2_NoQuestSpawned()
		{
			_gameState.PallOfSuffering = true;

			var result = _necromancerService.Activate(_gameState, roll: 2);

			Assert.IsFalse(result.SpawnQuest);
		}

		[Test]
		public void Spawn_NoPallOfSufferingRoll3_NoQuestSpawned()
		{
			_gameState.PallOfSuffering = false;

			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.IsFalse(result.SpawnQuest);
		}

		[Test]
		public void Spawn_PallOfSufferingRoll3_QuestSpawned()
		{
			_gameState.PallOfSuffering = true;

			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.IsTrue(result.SpawnQuest);
		}

		[Test]
		public void Spawn_FocusedRitualHeroesAtLocation_NoAdditionalBlights()
		{
			_gameState.Heroes.Active.Add(new Hero() { LocationId = LocationIds.Village });
			_gameState.Mode = DarknessCardsMode.Standard;
			_gameState.Necromancer.FocusedRituals = true;

			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(1, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_FocusedRitualNoHeroesAtLocation_AdditionalBlight()
		{
			_gameState.Heroes.Active.Add(new Hero() { LocationId = LocationIds.Ruins });
			_gameState.Mode = DarknessCardsMode.Standard;
			_gameState.Necromancer.FocusedRituals = true;

			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(2, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_CreepingShadowsRoll4_NoAdditionalBlight()
		{
			_gameState.Mode = DarknessCardsMode.Standard;
			_gameState.Necromancer.CreepingShadows = true;

			var result = _necromancerService.Activate(_gameState, roll:4);

			Assert.AreEqual(1, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_CreepingShadowsRoll5_AdditionalBlight()
		{
			_gameState.Mode = DarknessCardsMode.Standard;
			_gameState.Necromancer.CreepingShadows = true;

			var result = _necromancerService.Activate(_gameState, roll:5);

			Assert.AreEqual(2, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_CreepingShadowsRoll6_AdditionalBlight()
		{
			_gameState.Mode = DarknessCardsMode.Standard;
			_gameState.Necromancer.CreepingShadows = true;

			var result = _necromancerService.Activate(_gameState, roll:6);

			Assert.AreEqual(2, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_EncroachingShadowsRoll5_NoAdditionalBlightToMonastery()
		{
			_gameState.Mode = DarknessCardsMode.Standard;
			_gameState.Necromancer.EncroachingShadows = true;

			var result = _necromancerService.Activate(_gameState, roll:5);

			Assert.AreEqual(0, result.NumberOfBlightsToMonastery);
		}

		[Test]
		public void Spawn_EncroachingShadowsRoll6_AdditionalBlightToMonastery()
		{
			_gameState.Mode = DarknessCardsMode.Standard;
			_gameState.Necromancer.EncroachingShadows = true;

			var result = _necromancerService.Activate(_gameState, roll:6);

			Assert.AreEqual(1, result.NumberOfBlightsToMonastery);
		}

		[Test]
		public void Spawn_Overwhelm4thBlightNotAddedToNewLocation_NoAdditionalBlightToMonastery()
		{
			_gameState.Mode = DarknessCardsMode.Standard;
			_gameState.Necromancer.Overwhelm = true;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight> () {
				new Blight(),
				new Blight(),
			};
			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(0, result.NumberOfBlightsToMonastery);
		}

		[Test]
		public void Spawn_Overwhelm4thBlightAddedToNewLocation_AdditionalBlightToMonastery()
		{
			_gameState.Mode = DarknessCardsMode.Standard;
			_gameState.Necromancer.Overwhelm = true;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight> () {
				new Blight(),
				new Blight(),
				new Blight(),
			};
			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(1, result.NumberOfBlightsToMonastery);
		}

		[Test]
		public void Spawn_Overwhelm4thBlightAlreadyAtNewlocation_NoAdditionalBlightToMonastery()
		{
			_gameState.Mode = DarknessCardsMode.Standard;
			_gameState.Necromancer.Overwhelm = true;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight> () {
				new Blight(),
				new Blight(),
				new Blight(),
			};
			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(1, result.NumberOfBlightsToMonastery);
		}

		[Test]
		public void Spawn_DyingLand_NoDarknessTrackEffectsLocationEmpty_AdditionalBlight()
		{
			_gameState.Mode = DarknessCardsMode.Twilight;
			_gameState.Necromancer.DyingLand = true;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight> () {
			};
			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(2, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_DyingLand_NoDarknessTrackEffectsLocationNotEmpty_NoAdditionalBlight()
		{
			_gameState.Mode = DarknessCardsMode.Twilight;
			_gameState.Necromancer.DyingLand = true;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight> () {
				new Blight(),
			};
			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(1, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_DyingLand_DarknessTrackEffectsLocationEmpty_NoAdditionalBlight()
		{
			_gameState.Mode = DarknessCardsMode.Midnight;
			_gameState.Darkness = 10;
			_gameState.Necromancer.DyingLand = true;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight> () {
			};
			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(2, result.NumberOfBlightsToNewLocation);
		}

		[Test]
		public void Spawn_DyingLand_DarknessTrackEffectsLocationHasOneBlight_AdditionalBlight()
		{
			_gameState.Mode = DarknessCardsMode.Midnight;
			_gameState.Darkness = 10;
			_gameState.Necromancer.DyingLand = true;
			_gameState.Necromancer.LocationId = LocationIds.Ruins;
			_gameState.Locations.Single (x => x.Id == LocationIds.Village).Blights = new List<Blight> () {
				new Blight(),
			};
			var result = _necromancerService.Activate(_gameState, roll:3);

			Assert.AreEqual(2, result.NumberOfBlightsToNewLocation);
		}
	}
}

