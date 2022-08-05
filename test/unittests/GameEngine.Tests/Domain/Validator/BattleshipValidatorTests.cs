using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using GameEngine.Domain;
using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Validator;
using Xunit;

namespace GameEngine.Tests.Domain.Validator
{
    public class BattleshipValidatorTests
    {
        private readonly Fixture _fixture;
        
        public BattleshipValidatorTests()
        {
            _fixture = new Fixture();
        }
        
        [Theory]
        [InlineData(GameStatus.BattleInProgress)]
        [InlineData(GameStatus.Finished)]
        public void Should_throw_exception_if_game_status_is_invalid(GameStatus invalidGameStatus)
        {
            //given
            var gameId = Guid.NewGuid().ToString();
            var gameSettings = new GameSettings
            {
                GameStatus = invalidGameStatus
            };

            var battleship = new Battleship
            {
                GameId = gameId
            };

            var battleshipValidator = new BattleshipValidator(gameSettings, new List<Battleship>());
            
            //when
            Action act = () => battleshipValidator.ThrowExceptionIfBattleshipIsNotValid(battleship);

            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage($"Can't set battleship. Game status is '{invalidGameStatus.ToString()}'.");
        }

        [Fact]
        public void Should_throw_exception_if_all_battleships_exists()
        {
            //given
            var battleships = _fixture.CreateMany<Battleship>(3).ToList();
            
            var battleshipValidator = CreateBattleshipValidator(battleships);

            //when
            Action act = () => battleshipValidator.ThrowExceptionIfBattleshipIsNotValid(new Battleship());

            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Can't set battleship. You can have '3' ships only.");
        }
        
        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(1, 100)]
        [InlineData(100, 1)]
        [InlineData(100, 100)]
        public void Should_throw_exception_if_start_coordinates_are_invalid(int row, int column)
        {
            //given
            var battleshipValidator = CreateBattleshipValidator();

            //when
            Action act = () => battleshipValidator.ThrowExceptionIfBattleshipIsNotValid(new Battleship
            {
                Start = new Coordinates
                {
                    Row = row,
                    Column = column
                },
                End = new Coordinates
                {
                    Row = 1,
                    Column = 10
                }
            });

            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Can't set battleship. Coordinates are invalid.");
        }
        
        [Theory]
        [InlineData(-1, -1)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(1, 100)]
        [InlineData(100, 1)]
        [InlineData(100, 100)]
        public void Should_throw_exception_if_end_coordinates_are_invalid(int row, int column)
        {
            //given
            var battleshipValidator = CreateBattleshipValidator();

            //when
            Action act = () => battleshipValidator.ThrowExceptionIfBattleshipIsNotValid(new Battleship
            {
                Start = new Coordinates
                {
                    Row = 1,
                    Column = 10
                },
                End = new Coordinates
                {
                    Row = row,
                    Column = column
                }
            });

            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Can't set battleship. Coordinates are invalid.");
        }
        
        [Fact]
        public void Should_throw_exception_if_start_and_end_coordinates_are_not_in_line()
        {
            //given
            var battleshipValidator = CreateBattleshipValidator();

            //when
            Action act = () => battleshipValidator.ThrowExceptionIfBattleshipIsNotValid(new Battleship
            {
                Start = new Coordinates
                {
                    Row = 1,
                    Column = 10
                },
                End = new Coordinates
                {
                    Row = 2,
                    Column = 9
                }
            });

            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Can't set battleship. Coordinates are invalid.");
        }
        
        [Fact]
        public void Should_throw_exception_if_battleship_is_too_long()
        {
            //given
            var battleshipValidator = CreateBattleshipValidator();

            //when
            Action act = () => battleshipValidator.ThrowExceptionIfBattleshipIsNotValid(new Battleship
            {
                Start = new Coordinates
                {
                    Row = 1,
                    Column = 10
                },
                End = new Coordinates
                {
                    Row = 5,
                    Column = 10
                }
            });

            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Can't set battleship. Coordinates are invalid.");
        }
        
        [Fact]
        public void Should_throw_exception_if_battleship_is_too_short()
        {
            //given
            var battleshipValidator = CreateBattleshipValidator();

            //when
            Action act = () => battleshipValidator.ThrowExceptionIfBattleshipIsNotValid(new Battleship
            {
                Start = new Coordinates
                {
                    Row = 2,
                    Column = 5
                },
                End = new Coordinates
                {
                    Row = 2,
                    Column = 5
                }
            });

            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Can't set battleship. Coordinates are invalid.");
        }

        [Fact]
        public void Should_throw_exception_if_battleship_positions_overlap_with_other_battleship()
        {
            //given
            var existingBattleship = new Battleship
            {
                Start = new Coordinates
                {
                    Row = 2,
                    Column = 5
                },
                End = new Coordinates
                {
                    Row = 5,
                    Column = 5
                }
            };
            
            var newBattleship = new Battleship
            {
                Start = new Coordinates
                {
                    Row = 2,
                    Column = 5
                },
                End = new Coordinates
                {
                    Row = 2,
                    Column = 9
                }
            };
            
            var battleshipValidator = CreateBattleshipValidator(new List<Battleship>{ existingBattleship });
            
            //when
            Action act = () => battleshipValidator.ThrowExceptionIfBattleshipIsNotValid(newBattleship);

            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Can't set battleship. Positions overlap.");
        }

        [Fact]
        public void Should_not_throw_exception_if_battleship_is_valid()
        {
            //given
            var battleshipValidator = CreateBattleshipValidator();

            //when
            Action act = () => battleshipValidator.ThrowExceptionIfBattleshipIsNotValid(new Battleship
            {
                Start = new Coordinates
                {
                    Row = 2,
                    Column = 5
                },
                End = new Coordinates
                {
                    Row = 5,
                    Column = 5
                }
            });

            //then
            act
                .Should()
                .NotThrow<ValidationException>();
        }
        
        private static BattleshipValidator CreateBattleshipValidator(IEnumerable<Battleship> battleships)
        {
            var gameSettings = new GameSettings
            {
                Id = NewId(),
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int>{ 4, 4, 5 },
                GameStatus = GameStatus.SetupBattleships,
                NumberOfPlayers = 2
            };

            return new BattleshipValidator(gameSettings, battleships);
        }
        
        private static BattleshipValidator CreateBattleshipValidator()
        {
            var gameId = NewId();
            
            var gameSettings = new GameSettings
            {
                Id = gameId,
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int> { 4, 4, 5 },
                GameStatus = GameStatus.SetupBattleships,
                NumberOfPlayers = 2
            };
            
            var playerId = NewId();

            var battleships = new[]
            {
                new Battleship
                {
                    Id = NewId(),
                    GameId = gameId,
                    Start = new Coordinates
                    {
                        Row = 0,
                        Column = 4
                    },
                    End = new Coordinates
                    {
                        Row = 4,
                        Column = 4
                    },
                    Status = BattleshipStatus.Healthy,
                    PlayerId = playerId
                },
                new Battleship
                {
                    Id = NewId(),
                    GameId = gameId,
                    Start = new Coordinates
                    {
                        Row = 9,
                        Column = 1
                    },
                    End = new Coordinates
                    {
                        Row = 9,
                        Column = 5
                    },
                    Status = BattleshipStatus.Healthy,
                    PlayerId = playerId
                }
            };

            return new BattleshipValidator(gameSettings, battleships);
        }
        
        private static string NewId() => new IdGenerator().New();
    }
}

