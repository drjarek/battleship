using System.Collections.Generic;
using FluentAssertions;
using GameEngine.Domain;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Validator;
using Xunit;

namespace GameEngine.Tests.Domain.Validator
{
    public class BattleshipPositionValidatorTests
    {
        [Fact]
        public void Should_return_false_if_battleship_with_same_coordinates_exists()
        {
            //given
            var gameId = NewId();
            
            var existingBattleships = new List<Battleship>
            {
                new()
                {
                    Id = NewId(),
                    GameId = gameId,
                    Start = new Coordinates
                    {
                        Row = 0,
                        Column = 0
                    },
                    End = new Coordinates
                    {
                        Row = 0,
                        Column = 4
                    },
                    Status = BattleshipStatus.Healthy,
                    PlayerId = NewId()
                }
            };

            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = gameId,
                Start = new Coordinates
                {
                    Row = 0,
                    Column = 2
                },
                End = new Coordinates
                {
                    Row = 4,
                    Column = 2
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = new BattleshipPositionValidator(existingBattleships);
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Should_return_true_for_valid_battleship()
        {
            //given
            var gameId = NewId();
            
            var existingBattleships = new List<Battleship>
            {
                new()
                {
                    Id = NewId(),
                    GameId = gameId,
                    Start = new Coordinates
                    {
                        Row = 0,
                        Column = 0
                    },
                    End = new Coordinates
                    {
                        Row = 0,
                        Column = 4
                    },
                    Status = BattleshipStatus.Healthy,
                    PlayerId = NewId()
                }
            };

            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = gameId,
                Start = new Coordinates
                {
                    Row = 3,
                    Column = 2
                },
                End = new Coordinates
                {
                    Row = 6,
                    Column = 2
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = new BattleshipPositionValidator(existingBattleships);
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeTrue();
        }
        
        private static string NewId() => new IdGenerator().New();
    }
}