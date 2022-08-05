using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;

namespace GameEngine.Tests.Domain.Service
{
    public class PlayerServiceTests
    {
        private readonly Fixture _fixture;

        private readonly string _gameId;
        
        public PlayerServiceTests()
        {
            _fixture = new Fixture();
            _gameId = Guid.NewGuid().ToString();
        }
        
        [Fact]
        public void Should_create_new_player()
        {
            //given
            var player = _fixture
                .Build<Player>()
                .With(x => x.GameId, _gameId)
                .Create();

            var gameSettings = _fixture
                .Build<GameSettings>()
                .With(x => x.GameStatus, GameStatus.SetupPlayers)
                .With(x => x.Id, _gameId)
                .With(x => x.NumberOfPlayers, 2)
                .Create();
            
            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock
                .Setup(x => x.Get(_gameId))
                .Returns(gameSettings);

            var playerRepositoryMock = new Mock<IPlayerRepository>();
            playerRepositoryMock
                .Setup(x => x.GetAll(_gameId))
                .Returns(new List<Player>());
            
            //when
            var service = new PlayerService(playerRepositoryMock.Object, gameServiceMock.Object);
            service.Create(player);
            
            //then
            playerRepositoryMock.Verify(x => x.Create(player), Times.Once);
        }

        [Fact]
        public void Should_throw_validation_exception_when_create_player()
        {
            //given
            var player = _fixture
                .Build<Player>()
                .With(x => x.GameId, _gameId)
                .Create();

            var gameSettings = _fixture
                .Build<GameSettings>()
                .With(x => x.GameStatus, GameStatus.Finished)
                .With(x => x.Id, _gameId)
                .With(x => x.NumberOfPlayers, 2)
                .Create();

            var gameServiceMock = new Mock<IGameService>();
            gameServiceMock
                .Setup(x => x.Get(_gameId))
                .Returns(gameSettings);

            var playerRepositoryMock = new Mock<IPlayerRepository>();
            playerRepositoryMock
                .Setup(x => x.GetAll(_gameId))
                .Returns(new List<Player>());

            var service = new PlayerService(playerRepositoryMock.Object, gameServiceMock.Object);

            //when

            Action act = () => service.Create(player);

            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Invalid model.");
        }

        [Fact]
        public void Should_return_all_players()
        {
            //given
            var players = _fixture
                .Build<Player>()
                .With(x => x.GameId, _gameId)
                .CreateMany(2)
                .ToList();
            
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            playerRepositoryMock
                .Setup(x => x.GetAll(_gameId))
                .Returns(players);

            var gameServiceMock = new Mock<IGameService>();
            
            //when
            var service = new PlayerService(playerRepositoryMock.Object, gameServiceMock.Object);
            var actualPlayers = service.GetAll(_gameId);
            
            //then
            actualPlayers.Should().BeEquivalentTo(players);
  
        }
        
        [Fact]
        public void Should_return_empty_set_if_players_not_exists()
        {
            //given
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var gameServiceMock = new Mock<IGameService>();
            
            //when
            var service = new PlayerService(playerRepositoryMock.Object, gameServiceMock.Object);
            var actualPlayers = service.GetAll(_gameId);
            
            //then
            actualPlayers.Should().BeEmpty();
        }
    }
}