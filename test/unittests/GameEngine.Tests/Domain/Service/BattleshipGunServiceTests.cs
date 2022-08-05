using System;
using System.Collections.Generic;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;

namespace GameEngine.Tests.Domain.Service
{
    public class BattleshipGunServiceTests
    {
        private static string NewId() => Guid.NewGuid().ToString();

        [Fact]
        public void Should_fire_missile()
        {
            //given
            var gameId = NewId();
            var playerId = NewId();
            
            var missile = new Missile
            {
                Id = NewId(),
                GameId = gameId,
                PlayerId = playerId,
                Row = 0,
                Column = 0
            };

            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock
                .Setup(x => x.Get(gameId))
                .Returns(new GameSettings
                {
                    Id = gameId,
                    MaxRows = 10,
                    MaxColumns = 10,
                    AllowedBattleshipsSize = new List<int> { 3, 4 },
                    GameStatus = GameStatus.BattleInProgress,
                    NumberOfPlayers = 2
                });
            
            var missileRepositoryMock = new Mock<IMissileRepository>();
            missileRepositoryMock
                .Setup(x => x.GetAllPlayerShots(gameId, playerId))
                .Returns(new List<Missile>());

            var service = new BattleshipGunService(missileRepositoryMock.Object, gameServiceMock.Object);

            //when
            service.Fire(missile);
            
            //then
            missileRepositoryMock.Verify(x => x.Create(missile), Times.Once);
        }
        
        [Fact]
        public void Should_fire_missile_with_random_coordinates()
        {
            //given
            var gameId = NewId();
            var playerId = NewId();
            
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock
                .Setup(x => x.Get(gameId))
                .Returns(new GameSettings
                {
                    Id = gameId,
                    MaxRows = 10,
                    MaxColumns = 10,
                    AllowedBattleshipsSize = new List<int> { 3, 4 },
                    GameStatus = GameStatus.BattleInProgress,
                    NumberOfPlayers = 2
                });
            
            var missileRepositoryMock = new Mock<IMissileRepository>();
            missileRepositoryMock
                .Setup(x => x.GetAllPlayerShots(gameId, playerId))
                .Returns(new List<Missile>());

            var service = new BattleshipGunService(missileRepositoryMock.Object, gameServiceMock.Object);

            //when
            service.Fire(new MissileWithoutCoordinates
            {
                Id = NewId(),
                GameId = gameId,
                PlayerId = playerId
            });
            
            //then
            missileRepositoryMock.Verify(x => x.Create(It.IsAny<Missile>()), Times.Once);
        }
    }
}