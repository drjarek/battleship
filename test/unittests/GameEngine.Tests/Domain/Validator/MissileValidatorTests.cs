using System;
using System.Collections.Generic;
using FluentAssertions;
using GameEngine.Domain;
using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Validator;
using Xunit;

namespace GameEngine.Tests.Domain.Validator
{
    public class MissileValidatorTests
    {
        [Theory]
        [InlineData(GameStatus.SetupPlayers)]
        [InlineData(GameStatus.SetupBattleships)]
        [InlineData(GameStatus.Finished)]
        public void Should_throw_exception_if_game_status_is_invalid(GameStatus invalidGameStatus)
        {
            //given
            var gameSettings = new GameSettings
            {
                Id = NewId(),
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int> { 4, 4, 5 },
                GameStatus = invalidGameStatus,
                NumberOfPlayers = 2
            };

            var validator = new MissileValidator(gameSettings, new List<Missile>());
            
            //when
            Action act = () => validator.ThrowExceptionIfInvalid(new Missile());
            
            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Invalid model.");
        }
        
        [Fact]
        public void Should_throw_exception_if_missile_with_the_same_coordinates_exists()
        {
            //given
            var gameId = NewId();
            var playerId = NewId();
            
            var gameSettings = new GameSettings
            {
                Id = gameId,
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int> { 4, 4, 5 },
                GameStatus = GameStatus.BattleInProgress,
                NumberOfPlayers = 2
            };

            var missiles = new List<Missile>
            {
                new()
                {
                    Id = NewId(),
                    GameId = gameId,
                    PlayerId = playerId,
                    Row = 2,
                    Column = 2
                }
            };

            var validator = new MissileValidator(gameSettings, missiles);
            
            //when
            Action act = () => validator.ThrowExceptionIfInvalid(new Missile
            {
                Id = NewId(),
                GameId = gameId,
                PlayerId = playerId,
                Row = 2,
                Column = 2
            });
            
            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Invalid model.");
        }
        
        [Fact]
        public void Should_throw_exception_if_coordinates_are_invalid()
        {
            //given
            var gameId = NewId();
            var playerId = NewId();
            
            var gameSettings = new GameSettings
            {
                Id = gameId,
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int> { 4, 4, 5 },
                GameStatus = GameStatus.BattleInProgress,
                NumberOfPlayers = 2
            };

            var validator = new MissileValidator(gameSettings, new List<Missile>());
            
            //when
            Action act = () => validator.ThrowExceptionIfInvalid(new Missile
            {
                Id = NewId(),
                GameId = gameId,
                PlayerId = playerId,
                Row = -10,
                Column = -5
            });
            
            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage("Invalid model.");
        }
        
        private static string NewId() => new IdGenerator().New();
    }
}