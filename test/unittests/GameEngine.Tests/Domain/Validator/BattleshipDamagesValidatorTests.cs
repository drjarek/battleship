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
    public class BattleshipDamagesValidatorTests
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

            var validator = new BattleshipDamagesValidator(gameSettings);
            
            //when
            Action act = () => validator.ThrowExceptionIfIsInvalid();
            
            //then
            act
                .Should()
                .Throw<ValidationException>()
                .WithMessage($"Can't set damages. Game status is '{gameSettings.GameStatus.ToString()}'.");
        }
        
        [Fact]
        public void Should_not_throw_exception_if_game_status_is_valid()
        {
            //given
            var gameSettings = new GameSettings
            {
                Id = NewId(),
                MaxRows = 10,
                MaxColumns = 10,
                AllowedBattleshipsSize = new List<int> { 4, 4, 5 },
                GameStatus = GameStatus.BattleInProgress,
                NumberOfPlayers = 2
            };

            var validator = new BattleshipDamagesValidator(gameSettings);
            
            //when
            Action act = () => validator.ThrowExceptionIfIsInvalid();
            
            //then
            act
                .Should()
                .NotThrow<ValidationException>();
        }
        
        private static string NewId() => new IdGenerator().New();
    }
}