using System.Collections.Generic;
using FluentAssertions;
using GameEngine.Domain;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Validator;
using Xunit;

namespace GameEngine.Tests.Domain.Validator
{
    public class BattleshipCoordinatesValidatorTests
    {
        private const int MaxRows = 10;
        
        private const int MaxColumns = 10;

        private readonly string _gameId;

        public BattleshipCoordinatesValidatorTests()
        {
            _gameId = NewId();
        }

        [Fact]
        public void Should_return_false_if_start_row_is_less_than_zero()
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = -1,
                    Column = 2
                },
                End = new Coordinates
                {
                    Row = 2,
                    Column = 2
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(MaxRows)]
        [InlineData(MaxRows + 1)]
        public void Should_return_false_if_start_row_is_equal_or_greater_than_max_grid_row(int startRow)
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = startRow,
                    Column = 2
                },
                End = new Coordinates
                {
                    Row = 2,
                    Column = 2
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }
        
        [Fact]
        public void Should_return_false_if_start_column_is_less_than_zero()
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 5,
                    Column = -1
                },
                End = new Coordinates
                {
                    Row = 5,
                    Column = 2
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(MaxColumns)]
        [InlineData(MaxColumns + 1)]
        public void Should_return_false_if_start_column_is_equal_or_greater_then_max_grid_row(int startColumn)
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 2,
                    Column = startColumn
                },
                End = new Coordinates
                {
                    Row = 2,
                    Column = 2
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }
        
        [Fact]
        public void Should_return_false_if_end_row_is_less_than_zero()
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 3,
                    Column = 2
                },
                End = new Coordinates
                {
                    Row = -2,
                    Column = 2
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(MaxRows)]
        [InlineData(MaxRows + 1)]
        public void Should_return_false_if_end_row_is_equal_or_greater_than_max_grid_row(int endRow)
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 3,
                    Column = 2
                },
                End = new Coordinates
                {
                    Row = endRow,
                    Column = 2
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }
        
        [Fact]
        public void Should_return_false_if_end_column_is_less_than_zero()
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 5,
                    Column = 2
                },
                End = new Coordinates
                {
                    Row = 5,
                    Column = -3
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }
        
        [Theory]
        [InlineData(MaxColumns)]
        [InlineData(MaxColumns + 1)]
        public void Should_return_false_if_end_column_is_equal_or_greater_then_max_grid_row(int endColumn)
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 2,
                    Column = 3
                },
                End = new Coordinates
                {
                    Row = 2,
                    Column = endColumn
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Should_return_false_if_battleship_is_too_small()
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 0,
                    Column = 0
                },
                End = new Coordinates
                {
                    Row = 0,
                    Column = 2
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }
        
        [Fact]
        public void Should_return_false_if_battleship_is_too_big()
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 0,
                    Column = 0
                },
                End = new Coordinates
                {
                    Row = 0,
                    Column = 9
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Should_return_false_if_start_coordinates_are_greater_than_end_coordinates()
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 7,
                    Column = 8
                },
                End = new Coordinates
                {
                    Row = 4,
                    Column = 5
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeFalse();
        }

        [Fact]
        public void Should_return_true_if_battleship_is_valid()
        {
            //given
            var battleship = new Battleship
            {
                Id = NewId(),
                GameId = _gameId,
                Start = new Coordinates
                {
                    Row = 6,
                    Column = 8
                },
                End = new Coordinates
                {
                    Row = 9,
                    Column = 8
                },
                Status = BattleshipStatus.Healthy,
                PlayerId = NewId()
            };

            var validator = CreateBattleshipCoordinatesValidator();
            
            //when
            var isValid = validator.IsValid(battleship);
            
            //then
            isValid.Should().BeTrue();
        }

        private BattleshipCoordinatesValidator CreateBattleshipCoordinatesValidator()
        {
            var gameSettings = new GameSettings
            {
                Id = _gameId,
                MaxRows = MaxRows,
                MaxColumns = MaxColumns,
                AllowedBattleshipsSize = new List<int>{ 4, 4, 5 },
                GameStatus = GameStatus.BattleInProgress,
                NumberOfPlayers = 2
            };
            
            return new BattleshipCoordinatesValidator(gameSettings);
        }

        private static string NewId() => new IdGenerator().New();
    }
}