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
    public class PlayerRepositoryTests
    {
        private readonly Fixture _fixture;

        public PlayerRepositoryTests()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Should_create_new_record()
        {
            //given
            var player = _fixture.Create<Player>();
            
            //when
            var repository = new PlayerRepository();
            repository.Create(player);
            
            //then
            var players = repository.GetAll(player.GameId).ToList();
            players.Should().HaveCount(1);
            
            var actualPlayer = players.FirstOrDefault();
            actualPlayer.Should().BeEquivalentTo(player);
        }
        
        [Fact]
        public void Should_return_all_players()
        {
            //given
            var gameId = Guid.NewGuid().ToString();

            var player1 = _fixture
                .Build<Player>()
                .With(x => x.GameId, gameId)
                .Create();
            
            var player2 = _fixture
                .Build<Player>()
                .With(x => x.GameId, gameId)
                .Create();
            
            var repository = new PlayerRepository();
            repository.Create(player1);
            repository.Create(player2);
            
            //when
            var players = repository.GetAll(gameId).ToList();
            
            //then
            players.Should().HaveCount(2);
            players.Should().BeEquivalentTo(new List<Player> { player1, player2 });
        }
        
        [Fact]
        public void Should_return_empty_collection_if_players_not_exists()
        {
            //given
            var gameId = Guid.NewGuid().ToString();

            //when
            var repository = new PlayerRepository();
            var players = repository.GetAll(gameId);
            
            //then

            players.Should().BeEmpty();
        }
        
    }
}