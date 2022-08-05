using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using GameEngine.Domain.Repository.Model;
using GameEngine.Infrastructure.InMemoryRepository;
using Xunit;

namespace GameEngine.Tests.Infrastructure.Repository
{
    public class BattleshipRepositoryTests
    {
        [Fact]
        public void Should_create_new_battleship()
        {
            //given
            var battleship = CreateBattleship();
            var repository = new BattleshipRepository();
            
            //when
            repository.Create(battleship);
            
            //then
            var actualBattleship = repository.Get(battleship.GameId, battleship.Id);
            actualBattleship.Should().BeEquivalentTo(battleship);
        }

        [Fact]
        public void Should_overwrite_existing_record()
        {
            //given
            var battleship = CreateBattleship();
            
            var repository = new BattleshipRepository();
            repository.Create(battleship);

            var updatedBattleship = new Battleship
            {
                Id = battleship.Id,
                GameId = battleship.GameId,
                Start = battleship.Start,
                End = battleship.End,
                Status = BattleshipStatus.Destroyed,
                PlayerId = battleship.PlayerId
            };
            
            //when
            repository.Update(updatedBattleship);
            
            //then
            var actualBattleship = repository.Get(updatedBattleship.GameId, updatedBattleship.Id);
            actualBattleship.Should().BeEquivalentTo(updatedBattleship);
        }

        [Fact]
        public void Should_return_battleship_if_exist()
        {
            //given
            var expectedBattleship = CreateBattleship();
            
            var repository = new BattleshipRepository();
            repository.Create(expectedBattleship);
            
            //when
            var actual = repository.Get(expectedBattleship.GameId, expectedBattleship.Id);
            
            //then
            actual.Should().BeEquivalentTo(expectedBattleship);
        }
        
        [Fact]
        public void Should_not_return_battleship_if_not_exist()
        {
            //given
            var expectedBattleship = CreateBattleship();
            
            var repository = new BattleshipRepository();
            repository.Create(expectedBattleship);

            //when
            var actual = repository.Get(expectedBattleship.GameId, "not-existing-id");
            
            //then
            actual.Should().BeNull();
        }

        [Fact]
        public void Should_return_all_battleships_for_game()
        {
            //given
            var gameOneId = Guid.NewGuid().ToString();
            var gameTwoId = Guid.NewGuid().ToString();
            
            var repository = new BattleshipRepository();
            repository.Create(CreateBattleship(gameOneId));
            repository.Create(CreateBattleship(gameOneId));
            repository.Create(CreateBattleship(gameTwoId));

            //when
            var gameOneBattleships = repository.GetAll(gameOneId).ToList();
            var gameTwoBattleships = repository.GetAll(gameTwoId).ToList();

            //then
            gameOneBattleships.Should().HaveCount(2);
            gameTwoBattleships.Should().HaveCount(1);
        }
        
        [Fact] public void Should_not_return_battleships_for_game_if_not_exists()
        {
            //given
            var gameOneId = Guid.NewGuid().ToString();
            var gameTwoId = Guid.NewGuid().ToString();
            
            var repository = new BattleshipRepository();
            repository.Create(CreateBattleship(gameOneId));
            
            //when
            var gameTwoBattleships = repository.GetAll(gameTwoId).ToList();
            
            //then
            gameTwoBattleships.Should().HaveCount(0);
        }
        
        [Fact]
        public void Should_return_all_player_battleships()
        {
            //given
            var gameId = Guid.NewGuid().ToString();
            var playerOneId = Guid.NewGuid().ToString();
            var playerTwoId = Guid.NewGuid().ToString();

            var battleship1 = CreateBattleship(gameId, playerOneId);
            var battleship2 = CreateBattleship(gameId, playerOneId);
            var battleship3 = CreateBattleship(gameId, playerTwoId);
            
            var repository = new BattleshipRepository();
            repository.Create(battleship1);
            repository.Create(battleship2);
            repository.Create(battleship3);

            //when
            var battleships = repository.GetAllPlayerBattleships(gameId, playerOneId).ToList();

            //then
            battleships.Should().HaveCount(2);
            battleships.Should().BeEquivalentTo(new List<Battleship> { battleship1, battleship2 });
        }
        
        [Fact]
        public void Should_not_return_player_battleships_if_not_exists()
        {
            
            //given
            var gameId = Guid.NewGuid().ToString();
            var playerOneId = Guid.NewGuid().ToString();
            var playerTwoId = Guid.NewGuid().ToString();
            
            var repository = new BattleshipRepository();
            repository.Create(CreateBattleship(gameId, playerOneId));
            repository.Create(CreateBattleship(gameId, playerOneId));

            //when
            var battleships = repository.GetAllPlayerBattleships(gameId, playerTwoId);

            //then
            battleships.Should().HaveCount(0);
        }
        
        [Fact]
        public void Should_return_all_enemy_battleships()
        {
            //given
            var gameId = Guid.NewGuid().ToString();
            var playerOneId = Guid.NewGuid().ToString();
            var playerTwoId = Guid.NewGuid().ToString();
            
            var battleship1 = CreateBattleship(gameId, playerOneId);
            var battleship2 = CreateBattleship(gameId, playerOneId);
            var battleship3 = CreateBattleship(gameId, playerTwoId);
            
            var repository = new BattleshipRepository();
            repository.Create(battleship1);
            repository.Create(battleship2);
            repository.Create(battleship3);

            //when
            var battleships = repository.GetAllEnemyBattleships(gameId, playerOneId).ToList();

            //then
            battleships.Should().HaveCount(1);
            battleships.Should().BeEquivalentTo(new List<Battleship> { battleship3 });
        }
        
        [Fact]
        public void Should_not_return_enemy_battleships_if_not_exists()
        {
            //given
            var gameId = Guid.NewGuid().ToString();
            var playerOneId = Guid.NewGuid().ToString();

            var repository = new BattleshipRepository();
            repository.Create(CreateBattleship(gameId, playerOneId));
            repository.Create(CreateBattleship(gameId, playerOneId));

            //when
            var battleships = repository.GetAllEnemyBattleships(gameId, playerOneId);

            //then
            battleships.Should().HaveCount(0);
        }

        private static Battleship CreateBattleship()
        {
            return CreateBattleship(Guid.NewGuid().ToString());
        }
        
        private static Battleship CreateBattleship(string gameId)
        {
            return CreateBattleship(gameId, Guid.NewGuid().ToString());
        }

        private static Battleship CreateBattleship(string gameId, string playerId)
        {
            var fixture = new Fixture();
            var battleship = fixture
                .Build<Battleship>()
                .With(x => x.GameId, gameId)
                .With(x => x.PlayerId, playerId)
                .With(x => x.Status, BattleshipStatus.Healthy)
                .Create();

            return battleship;
        }
    }
}