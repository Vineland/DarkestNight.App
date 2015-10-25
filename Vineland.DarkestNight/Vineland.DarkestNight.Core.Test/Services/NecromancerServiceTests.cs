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
            Assert.AreEqual(6, result.Roll);
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
        public void Detect_MultipleHerosVisible_OneHeroChosen()
        {
            //arrange
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 1 });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 6, Id = 2 });
            _gameState.Heroes.Active.Add(new Hero() { Secrecy = 5, Id = 3 });

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.IsTrue(result.DetectedHeroId == 1 || result.DetectedHeroId == 3);
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

            //act
            var result = _necromancerService.Detect(_gameState);

            //assert
            Assert.AreEqual(1, result.DetectedHeroId);
        }

        [TestMethod]
        public void Detect_AuroOfHumilityActiveAtMountains_NoDetections()
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
    }
}
