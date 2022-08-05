using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GameEngine.Domain;
using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;

namespace GameEngine.Tests.Domain.Service
{
    public class BattleshipServiceTests
    {
        private readonly string _gameId;

        public BattleshipServiceTests()
        {
            _gameId = NewId();
        }
        
        [Fact]
        public void Should_create_new_battleship()
        {
            //given
            var playerId = NewId();
            
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 0,
                    Column = 2
                },
                End = new Coordinates
                {
                    Row = 0,
                    Column = 4
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = playerId
            };
            
            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAllPlayerBattleships(_gameId, playerId))
                .Returns(new List<Battleship>());
            
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock
                .Setup(x => x.Get(_gameId))
                .Returns(new GameSettings
                {
                    Id = _gameId,
                    MaxRows = 10,
                    MaxColumns = 10,
                    AllowedBattleshipsSize = new List<int>{ 3, 4 },
                    GameStatus = GameStatus.SetupBattleships,
                    NumberOfPlayers = 2
                });

            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            service.Create(battleship);
            
            //then
            battleshipRepositoryMock.Verify(x => x.Create(battleship), Times.Once);
        }
        
        [Fact]
        public void Should_throw_exception_when_create_battleship_if_battleship_is_invalid()
        {
            //given
            var playerId = NewId();
            
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = -1,
                    Column = -2
                },
                End = new Coordinates
                {
                    Row = -1,
                    Column = -4
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = playerId
            };
            
            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAllPlayerBattleships(_gameId, playerId))
                .Returns(new List<Battleship>());
            
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock
                .Setup(x => x.Get(_gameId))
                .Returns(new GameSettings
                {
                    Id = _gameId,
                    MaxRows = 10,
                    MaxColumns = 10,
                    AllowedBattleshipsSize = new List<int>{ 3, 4 },
                    GameStatus = GameStatus.SetupBattleships,
                    NumberOfPlayers = 2
                });

            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            Action act = () => service.Create(battleship);
            
            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Can't set battleship. Coordinates are invalid.");
        }

        [Fact]
        public void Should_create_new_battleship_with_random_position()
        {
            //given
            var playerId = NewId();
            
            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAllPlayerBattleships(_gameId, playerId))
                .Returns(new List<Battleship>());
            
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock
                .Setup(x => x.Get(_gameId))
                .Returns(new GameSettings
                {
                    Id = _gameId,
                    MaxRows = 10,
                    MaxColumns = 10,
                    AllowedBattleshipsSize = new List<int>{ 3, 4 },
                    GameStatus = GameStatus.SetupBattleships,
                    NumberOfPlayers = 2
                });
            
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 0,
                    Column = 2
                },
                End = new Coordinates
                {
                    Row = 0,
                    Column = 4
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = playerId
            };

            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            battleshipGeneratorMock
                .Setup(x => x.Generate(3, playerId, _gameId))
                .Returns(battleship);
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            service.Create(3, _gameId, playerId);
            
            //then
            battleshipRepositoryMock.Verify(x => x.Create(It.IsAny<Battleship>()), Times.Once);
        }

        [Fact]
        public void Should_add_damages_if_hit_battleship()
        {
            //given
            var playerId = NewId();

            var missileCoordinates = new Coordinates
            {
                Row = 4,
                Column = 5
            };

            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 4,
                    Column = 5
                },
                End = new Coordinates
                {
                    Row = 4,
                    Column = 7
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };
            
            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAllEnemyBattleships(_gameId, playerId))
                .Returns(new List<Battleship>{ battleship });
            
            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();

            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock
                .Setup(x => x.Get(_gameId))
                .Returns(new GameSettings
                {
                    Id = _gameId,
                    MaxRows = 10,
                    MaxColumns = 10,
                    AllowedBattleshipsSize = new List<int> { 4, 4, 5 },
                    GameStatus = GameStatus.BattleInProgress,
                    NumberOfPlayers = 2
                });
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            service.AddDamagesIfHit(missileCoordinates, _gameId, playerId);
            
            //then
            battleshipRepositoryMock.Verify(x => x.Update(It.IsAny<Battleship>()), Times.Once);
        }
        
        [Fact]
        public void Should_not_add_damages_if_battleship_was_not_hit()
        {
            //given
            var playerId = NewId();

            var missileCoordinates = new Coordinates
            {
                Row = 0,
                Column = 0
            };

            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 4,
                    Column = 5
                },
                End = new Coordinates
                {
                    Row = 4,
                    Column = 7
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };
            
            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAllEnemyBattleships(_gameId, playerId))
                .Returns(new List<Battleship>{ battleship });
            
            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock
                .Setup(x => x.Get(_gameId))
                .Returns(new GameSettings
                {
                    Id = _gameId,
                    MaxRows = 10,
                    MaxColumns = 10,
                    AllowedBattleshipsSize = new List<int> { 4, 4, 5 },
                    GameStatus = GameStatus.BattleInProgress,
                    NumberOfPlayers = 2
                });
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            service.AddDamagesIfHit(missileCoordinates, _gameId, playerId);
            
            //then
            battleshipRepositoryMock.Verify(x => x.Update(It.IsAny<Battleship>()), Times.Never);
        }

        [Fact]
        public void Should_throw_exception_if_try_add_the_same_damages_again()
        {
            //given
            var playerId = NewId();

            var missileCoordinates = new Coordinates
            {
                Row = 4,
                Column = 5
            };

            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 4,
                    Column = 5
                },
                End = new Coordinates
                {
                    Row = 4,
                    Column = 7
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId(),
                Damages = { new Coordinates
                    {
                        Row = 4,
                        Column = 5
                    }
                }
            };
            
            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAllEnemyBattleships(_gameId, playerId))
                .Returns(new List<Battleship>{ battleship });
            
            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock
                .Setup(x => x.Get(_gameId))
                .Returns(new GameSettings
                {
                    Id = _gameId,
                    MaxRows = 10,
                    MaxColumns = 10,
                    AllowedBattleshipsSize = new List<int> { 4, 4, 5 },
                    GameStatus = GameStatus.BattleInProgress,
                    NumberOfPlayers = 2
                });
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            Action act = () => service.AddDamagesIfHit(missileCoordinates, _gameId, playerId);
            
            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Can't set damages. Can't duplicate damages");
        }
        
        [Fact]
        public void Should_return_all_battleships()
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 4,
                    Column = 5
                },
                End = new Coordinates
                {
                    Row = 4,
                    Column = 7
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAll(_gameId))
                .Returns(new List<Battleship>{ battleship });
            
            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            
            var gameServiceMock = new Mock<IGameService>();
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            var actualBattleships = service.GetAll(_gameId).ToList();
            
            //then
            actualBattleships.Should().HaveCount(1);
            actualBattleships.FirstOrDefault().Should().BeEquivalentTo(battleship);
        }

        [Fact]
        public void Should_return_empty_set_if_battleships_have_not_been_created()
        {
            //given
            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAll(_gameId))
                .Returns(new List<Battleship>());
            
            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            
            var gameServiceMock = new Mock<IGameService>();
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            var actualBattleships = service.GetAll(_gameId).ToList();
            
            //then
            actualBattleships.Should().BeEmpty();
        }
        
        [Fact]
        public void Should_return_all_player_battleships()
        {
            //given
            var playerId = NewId();
            
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 4,
                    Column = 5
                },
                End = new Coordinates
                {
                    Row = 4,
                    Column = 7
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = playerId
            };

            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAllPlayerBattleships(_gameId, playerId))
                .Returns(new List<Battleship>{ battleship });
            
            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            
            var gameServiceMock = new Mock<IGameService>();
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            var actualBattleships = service.GetAll(_gameId, playerId).ToList();
            
            //then
            actualBattleships.Should().HaveCount(1);
            actualBattleships.FirstOrDefault().Should().BeEquivalentTo(battleship);
        }
        
        [Fact]
        public void Should_return_empty_set_if_player_has_not_created_any_battleships()
        {
            //given
            var playerId = NewId();

            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAllPlayerBattleships(_gameId, playerId))
                .Returns(new List<Battleship>());
            
            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            
            var gameServiceMock = new Mock<IGameService>();
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            var actualBattleships = service.GetAll(_gameId, playerId).ToList();
            
            //then
            actualBattleships.Should().BeEmpty();
        }
        
        [Fact]
        public void Should_return_all_enemy_battleships()
        {
            //given
            var playerId = NewId();
            var enemyId = NewId();
            
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 4,
                    Column = 5
                },
                End = new Coordinates
                {
                    Row = 4,
                    Column = 7
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = enemyId
            };

            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAllEnemyBattleships(_gameId, playerId))
                .Returns(new List<Battleship>{ battleship });
            
            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            
            var gameServiceMock = new Mock<IGameService>();
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            var actualBattleships = service.GetAllEnemyBattleships(_gameId, playerId).ToList();
            
            //then
            actualBattleships.Should().HaveCount(1);
            actualBattleships.FirstOrDefault().Should().BeEquivalentTo(battleship);
        }
        
        [Fact]
        public void Should_return_empty_set_if_enemy__has_net_created_any_battleships()
        {
            //given
            var playerId = NewId();

            var battleshipRepositoryMock = new Mock<IBattleshipRepository>();
            battleshipRepositoryMock
                .Setup(x => x.GetAllEnemyBattleships(_gameId, playerId))
                .Returns(new List<Battleship>());
            
            var battleshipGeneratorMock = new Mock<IBattleshipGenerator>();
            
            var gameServiceMock = new Mock<IGameService>();
            
            var service = new BattleshipService(battleshipRepositoryMock.Object, battleshipGeneratorMock.Object,
                gameServiceMock.Object);
            
            //when
            var actualBattleships = service.GetAllEnemyBattleships(_gameId, playerId).ToList();
            
            //then
            actualBattleships.Should().BeEmpty();
        }

        private static string NewId() => new IdGenerator().New();
    }
}

