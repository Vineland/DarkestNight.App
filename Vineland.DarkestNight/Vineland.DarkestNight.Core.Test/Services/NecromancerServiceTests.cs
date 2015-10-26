using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vineland.DarkestNight.Core.Services;
using Moq;

namespace Vineland.DarkestNight.Core.Test.Services
{
    [TestClass]
    public class NecromancerServiceTests
    {
        NecromancerService _necromancerService;
        GameState _gameState;
        Mock<D6GeneratorService> _mockD6Service;

        [TestInitialize]
        public void Setup()
        {
            _mockD6Service = new Mock<D6GeneratorService>();
            _mockD6Service.Setup(x => x.RollDemBones()).Returns(6);

            _necromancerService = new NecromancerService(null, _mockD6Service.Object);

            _gameState = new GameState();
        }

        [TestMethod]
        public void Detect_HeroSecrecy6Roll6_NoDetections()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 6 });

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.IsNull(result.DetectedHeroId);
            Assert.AreEqual(6, result.DetectionRoll);
        }

        [TestMethod]
        public void Detect_HeroSecrecy5Roll6_HeroDetected()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1 });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2 });

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.AreEqual(1, result.DetectedHeroId);
        }

        [TestMethod]
        public void Detect_HeroAtMonasterySecrecy5Roll6_NotDetected()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Monastery });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2 });

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.IsNull(result.DetectedHeroId);
        }

        [TestMethod]
        public void Detect_MultipleHeroesDetected_HeroAtCurrentLocationChosen()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1 , LocationId = LocationIds.Castle});
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2 , LocationId = LocationIds.Swamp});
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3 , LocationId = LocationIds.Ruins});
            _gameState.Necromancer.LocationId = LocationIds.Ruins;

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.AreEqual(3, result.DetectedHeroId);
        }

        [TestMethod]
        public void Detect_MultipleHeroesDetected_ClosestHeroChosen()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Castle });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 2, LocationId = LocationIds.Swamp });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 3, LocationId = LocationIds.Ruins });
            _gameState.Necromancer.LocationId = LocationIds.Ruins;

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.AreEqual(2, result.DetectedHeroId);
        }

        [TestMethod]
        public void Detect_MultipleHeroesDetected_RandomHeroOneDistanceAwayChosen()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Forest });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 2, LocationId = LocationIds.Swamp });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3, LocationId = LocationIds.Mountains });
            _gameState.Necromancer.LocationId = LocationIds.Ruins;

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.AreNotEqual(3, result.DetectedHeroId);
        }

        [TestMethod]
        public void Detect_MultipleHeroesDetected_RandomHeroDistanceTwoAwayChosen()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Swamp });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 2, LocationId = LocationIds.Castle });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3, LocationId = LocationIds.Monastery });
            _gameState.Necromancer.LocationId = LocationIds.Forest;

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.AreNotEqual(3, result.DetectedHeroId);
        }

        [TestMethod]
        public void Detect_MultipleHeroesDetectedAndGatesActive_RandomHeroChosen()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, LocationId = LocationIds.Castle });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 2, LocationId = LocationIds.Ruins });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3, LocationId = LocationIds.Forest });
            _gameState.Necromancer.LocationId = LocationIds.Ruins;
            _gameState.Necromancer.GatesActive = true;

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.IsTrue(result.DetectedHeroId == 1 || result.DetectedHeroId == 2 || result.DetectedHeroId == 3);
        }

        [TestMethod]
        public void Detect_RangerWithHermitAtSwamp_NotDetected()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, Name = "Ranger", LocationId = LocationIds.Swamp });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2 });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3 });
            _gameState.Heroes.HermitActive = true;

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.AreEqual(3, result.DetectedHeroId);
        }

        [TestMethod]
        public void Detect_RangerWithHermitNotSwamp_HeroDetected()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1, Name = "Ranger", LocationId = LocationIds.Castle });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2 });
            _gameState.Heroes.HermitActive = true;
            _gameState.Necromancer.LocationId = LocationIds.Ruins;

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.AreEqual(1, result.DetectedHeroId);
        }

        [TestMethod]
        public void Detect_AuraOfHumilityActiveAtMountains_NoDetections()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 1, Id = 1, LocationId = LocationIds.Mountains });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 1, Id = 2, LocationId = LocationIds.Mountains });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 1, Id = 3, LocationId = LocationIds.Mountains });
            _gameState.Heroes.AuraOfHumilityLocationId = LocationIds.Mountains;

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.IsNull(result.DetectedHeroId);

        }

        [TestMethod]
        public void Detect_HeroSecrecy6Roll6GatesActive_HeroDetected()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() {Id=1, Secrecy = 6 });
            _gameState.Necromancer.GatesActive = true;

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.AreEqual(1, result.DetectedHeroId);
            Assert.AreEqual(7, result.DetectionRoll);
        }
    }
}
