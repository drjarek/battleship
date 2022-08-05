using System;
using System.Collections.Generic;
using FluentAssertions;
using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;

namespace GameEngine.Tests.Domain.Service
{
    public class GameServiceTests
    {
        [Fact]
        public void Should_start_game()
        {
            //given
            var gameSettings = new GameSettings
            {
                GameStatus = GameStatus.SetupBattleships
            };

            var gameSettingsRepositoryMock = new Mock<IGameSettingsRepository>();
            
            var gameService = new GameService(gameSettingsRepositoryMock.Object);
            
            //when
            gameService.Create(gameSettings);

            //then
            gameSettingsRepositoryMock
                .Verify(x => x.Create(gameSettings), Times.Once);
        }

        [Fact]
        public void Should_return_game_settings_for_valid_id()
        {
            //given
            var id = Guid.NewGuid().ToString();

            var gameSettings = new GameSettings
            {
                Id = id,
                GameStatus = GameStatus.SetupPlayers,
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int>{3, 4},
                NumberOfPlayers = 2
            };

            var gameSettingsRepositoryMock = new Mock<IGameSettingsRepository>();
            gameSettingsRepositoryMock
                .Setup(x => x.Get(id))
                .Returns(gameSettings);
            
            var gameService = new GameService(gameSettingsRepositoryMock.Object);
            
            //when
            var actualGameSettings = gameService.Get(id);

            //then
            actualGameSettings.Should().BeEquivalentTo(gameSettings);
        }

        [Fact]
        public void Should_throw_validation_exceptions_if_game_settings_not_found()
        {
            //given
            var gameSettingsRepositoryMock = new Mock<IGameSettingsRepository>();
            gameSettingsRepositoryMock
                .Setup(x => x.Get("not-existing-id"))
                .Returns(null as GameSettings);
            
            var gameService = new GameService(gameSettingsRepositoryMock.Object);
            
            //when
            Action act = () => gameService.Get("not-existing-id");

            //then
            act.Should().Throw<NotFoundException>()
                .WithMessage("Game settings for 'not-existing-id' id not found");
        }

        [Fact]
        public void Should_set_SetupBattleships_status_if_all_players_have_been_created()
        {
            //given
            var gameId = Guid.NewGuid().ToString();
            const int numberOfPlayers = 2;

            var gameSettings = new GameSettings
            {
                Id = gameId,
                GameStatus = GameStatus.SetupPlayers,
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int>{3, 4},
                NumberOfPlayers = numberOfPlayers
            };

            var gameSettingsRepositoryMock = new Mock<IGameSettingsRepository>();
            gameSettingsRepositoryMock
                .Setup(x => x.Get(gameId))
                .Returns(gameSettings);

            var gameService = new GameService(gameSettingsRepositoryMock.Object);
            
            //when
            gameService.TrySetNewStatus(new SetupBattleshipStatus(gameId, numberOfPlayers));
            
            //then
            gameSettingsRepositoryMock
                .Verify(x => x.Update(It.IsAny<GameSettings>()), Times.Once);
        }
        
        [Fact]
        public void Should_not_set_SetupBattleships_status_if_not_all_players_have_been_created()
        {
            //given
            var gameId = Guid.NewGuid().ToString();
            const int numberOfPlayers = 0;

            var gameSettings = new GameSettings
            {
                Id = gameId,
                GameStatus = GameStatus.SetupPlayers,
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int>{3, 4},
                NumberOfPlayers = 2
            };

            var gameSettingsRepositoryMock = new Mock<IGameSettingsRepository>();
            gameSettingsRepositoryMock
                .Setup(x => x.Get(gameId))
                .Returns(gameSettings);

            var gameService = new GameService(gameSettingsRepositoryMock.Object);
            
            //when
            gameService.TrySetNewStatus(new SetupBattleshipStatus(gameId, numberOfPlayers));
            
            //then
            gameSettingsRepositoryMock
                .Verify(x => x.Update(It.IsAny<GameSettings>()), Times.Never);
        }
        
        [Fact]
        public void Should_set_BattleInProgressS_status_if_all_battleships_have_been_created()
        {
            //given
            var gameId = Guid.NewGuid().ToString();

            var gameSettings = new GameSettings
            {
                Id = gameId,
                GameStatus = GameStatus.SetupBattleships,
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int>{3, 4},
                NumberOfPlayers = 2
            };

            var gameSettingsRepositoryMock = new Mock<IGameSettingsRepository>();
            gameSettingsRepositoryMock
                .Setup(x => x.Get(gameId))
                .Returns(gameSettings);

            var gameService = new GameService(gameSettingsRepositoryMock.Object);
            
            //when
            gameService.TrySetNewStatus(new BattleInProgressStatus(gameId, 4));
            
            //then
            gameSettingsRepositoryMock
                .Verify(x => x.Update(It.IsAny<GameSettings>()), Times.Once);
        }
        
        [Fact]
        public void Should_not_set_BattleInProgressS_status_if_all_battleships_have_not_been_created()
        {
            //given
            var gameId = Guid.NewGuid().ToString();

            var gameSettings = new GameSettings
            {
                Id = gameId,
                GameStatus = GameStatus.SetupBattleships,
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int>{3, 4},
                NumberOfPlayers = 2
            };

            var gameSettingsRepositoryMock = new Mock<IGameSettingsRepository>();
            gameSettingsRepositoryMock
                .Setup(x => x.Get(gameId))
                .Returns(gameSettings);

            var gameService = new GameService(gameSettingsRepositoryMock.Object);
            
            //when
            gameService.TrySetNewStatus(new BattleInProgressStatus(gameId, 1));
            
            //then
            gameSettingsRepositoryMock
                .Verify(x => x.Update(It.IsAny<GameSettings>()), Times.Never);
        }
        
        [Fact]
        public void Should_set_Finished_status_if_all_enemy_battleships_have_been_destroyed()
        {
            //given
            var gameId = Guid.NewGuid().ToString();

            var gameSettings = new GameSettings
            {
                Id = gameId,
                GameStatus = GameStatus.BattleInProgress,
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int>{3, 4},
                NumberOfPlayers = 2
            };

            var gameSettingsRepositoryMock = new Mock<IGameSettingsRepository>();
            gameSettingsRepositoryMock
                .Setup(x => x.Get(gameId))
                .Returns(gameSettings);

            var gameService = new GameService(gameSettingsRepositoryMock.Object);
            
            //when
            gameService.TrySetNewStatus(new GameFinishedStatus(gameId, 2));
            
            //then
            gameSettingsRepositoryMock
                .Verify(x => x.Update(It.IsAny<GameSettings>()), Times.Once);
        }
        
        [Fact]
        public void Should_not_set_Finished_status_if_all_enemy_battleships_have_not_been_destroyed()
        {
            //given
            var gameId = Guid.NewGuid().ToString();

            var gameSettings = new GameSettings
            {
                Id = gameId,
                GameStatus = GameStatus.BattleInProgress,
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int>{3, 4},
                NumberOfPlayers = 2
            };

            var gameSettingsRepositoryMock = new Mock<IGameSettingsRepository>();
            gameSettingsRepositoryMock
                .Setup(x => x.Get(gameId))
                .Returns(gameSettings);

            var gameService = new GameService(gameSettingsRepositoryMock.Object);
            
            //when
            gameService.TrySetNewStatus(new GameFinishedStatus(gameId, 0));
            
            //then
            gameSettingsRepositoryMock
                .Verify(x => x.Update(It.IsAny<GameSettings>()), Times.Never);
        }
    }
}

