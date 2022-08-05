using System;
using System.Linq;
using AutoFixture;
using GameEngine.Application.GameApp.Notification;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;
using MissileFiredNotification = GameEngine.Application.GunApp.Notification.MissileFiredNotification;

namespace GameEngine.Tests.Application.GameApp.Notification
{
    public class MissileFiredNotificationProcessorTests
    {
        [Fact]
        public void Should_process_notification()
        {
            //given
            var notification = new MissileFiredNotification (
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                2,
                3
                );

            var gameServiceMock = new Mock<IGameService>();
            
            var fixture = new Fixture();
            var battleships = fixture
                .Build<Battleship>()
                .With(b => b.Status, BattleshipStatus.Destroyed)
                .CreateMany(4)
                .ToList();
            
            var battleshipServiceMock = new Mock<IBattleshipService>();
            battleshipServiceMock
                .Setup(x => x.GetAll(notification.GameId))
                .Returns(battleships);
            
            //when
            var processor =
                new MissileFiredNotificationProcessor<MissileFiredNotification>(battleshipServiceMock.Object, gameServiceMock.Object);
            
            processor.Process(notification);
            
            //then
            gameServiceMock
                .Verify(x => x.TrySetNewStatus(It.IsAny<GameFinishedStatus>()), Times.Once);
        }
    }
}