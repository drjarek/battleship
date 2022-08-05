using System.Linq;
using AutoFixture;
using FluentAssertions;
using GameEngine.Domain.Repository.Model;
using GameEngine.Infrastructure.InMemoryRepository;
using Xunit;

namespace GameEngine.Tests.Infrastructure.Repository
{
    public class MissileRepositoryTests
    {
        private readonly Fixture _fixture;

        public MissileRepositoryTests()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Should_create_new_record_and_return_player_fired_missiles()
        {
            //given
            var missile = _fixture.Create<Missile>();

            var repository = new MissileRepository();
            
            //when
            repository.Create(missile);
            
            //then
            var actualMissiles = repository.GetAllPlayerShots(missile.GameId, missile.PlayerId);

            actualMissiles.Should().HaveCount(1);
            actualMissiles.FirstOrDefault().Should().BeEquivalentTo(missile);
        }
    }
}