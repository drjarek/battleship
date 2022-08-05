using System.Collections.Generic;
using FluentAssertions;
using GameEngine.Domain;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Repository.Model.Extensions;
using Moq;
using Xunit;

namespace GameEngine.Tests.Domain
{
    public class BattleshipGeneratorTests
    {
        [Fact]
        public void Should_generate_battleship()
        {
            //given
            var gameId = NewId();
            var playerId = NewId();
            
            var gameSettingsRepositoryMock = new Mock<IGameSettingsRepository>();
            gameSettingsRepositoryMock
                .Setup(x => x.Get(gameId))
                .Returns(new GameSettings
                {
                    Id = gameId,
                    MaxRows = 10,
                    MaxColumns = 10,
                    AllowedBattleshipsSize = new List<int> { 4, 4, 5 },
                    GameStatus = GameStatus.BattleInProgress,
                    NumberOfPlayers = 2
                });

            var battleships = new List<Battleship>
            {
                new()
                {
                    Id = NewId(),
                    GameId = gameId,
                    Start = new Coordinates
                    {
                        Row = 1,
                        Column = 5
                    },
                    End = new Coordinates
                    {
                        Row = 1,
                        Column = 8
                    },
                    Status = BattleshipStatus.Healthy,
                    PlayerId = playerId
                },
                new()
                {
                    Id = NewId(),
                    GameId = gameId,
                    Start = new Coordinates
                    {
                        Row = 6,
                        Column = 2
                    },
                    End = new Coordinates
                    {
                        Row = 6,
                        Column = 5
                    },
                    Status = BattleshipStatus.Healthy,
                    PlayerId = playerId
                }
            };

            var battleshipRepository = new Mock<IBattleshipRepository>();
            battleshipRepository
                .Setup(x => x.GetAllPlayerBattleships(gameId, playerId))
                .Returns(battleships);
            
            var generator = new BattleshipGenerator(gameSettingsRepositoryMock.Object, battleshipRepository.Object,
                new IdGenerator());
            
            //when
            var battleship = generator.Generate(3, playerId, gameId);
            
            //then
            battleship.Should().BeAssignableTo<Battleship>();
            battleship.PlayerId.Should().Be(playerId);
            battleship.GameId.Should().Be(gameId);
            battleship.Size().Should().Be(3);
        }

        private static string NewId() => new IdGenerator().New();
    }
}

