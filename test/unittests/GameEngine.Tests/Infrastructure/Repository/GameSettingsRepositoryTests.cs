using System;
using AutoFixture;
using FluentAssertions;
using GameEngine.Domain.Repository.Model;
using GameEngine.Infrastructure.InMemoryRepository;
using Xunit;

namespace GameEngine.Tests.Infrastructure.Repository
{
    public class GameSettingsRepositoryTests
    {
        [Fact]
        public void Should_create_new_record()
        {
            //given
            var record = CreateGameSettings();
            
            //when
            var repository = new GameSettingsRepository();
            repository.Create(record);

            //then
            var actualRecord = repository.Get(record.Id);
            actualRecord.Should().BeEquivalentTo(record);
        }

        [Fact]
        public void Should_update_record()
        {
            //given
            var record = CreateGameSettings();
            
            var repository = new GameSettingsRepository();
            repository.Create(record);

            //when
            var updatedRecord = CreateGameSettings(record.Id);
            repository.Update(updatedRecord);

            //then
            var actualRecord = repository.Get(updatedRecord.Id);
            actualRecord.Should().BeEquivalentTo(updatedRecord);
        }

        [Fact]
        public void Should_return_record()
        {
            //given
            var record = CreateGameSettings();
            
            var repository = new GameSettingsRepository();
            repository.Create(record);

            //when
            var actualRecord = repository.Get(record.Id);
            
            //then
            actualRecord.Should().BeEquivalentTo(record);
        }
        
        [Fact]
        public void Should_return_null_if_record_not_exist()
        {
            //given
            var record = CreateGameSettings();
            
            var repository = new GameSettingsRepository();
            repository.Create(record);

            //when
            var actualRecord = repository.Get(Guid.NewGuid().ToString());
            
            //then
            actualRecord.Should().BeNull();
        }

        private static GameSettings CreateGameSettings()
        {
            return CreateGameSettings(Guid.NewGuid().ToString());
        }
        
        private static GameSettings CreateGameSettings(string id)
        {
            var fixture = new Fixture();
            return fixture
                .Build<GameSettings>()
                .With(gs => gs.Id, id)
                .Create();
        }
    }
}