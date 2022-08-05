using System;
using System.Linq;
using AutoFixture;
using GameEngine.Application.BattleshipApp.Notification;
using GameEngine.Application.GameApp.Notification;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;

namespace GameEngine.Tests.Application.GameApp.Notification
{
    public class BattleshipCreatedNotificationProcessorTests
    {
        [Fact]
        public void Should_process_notification()
        {
            //given
            var notification = new BattleshipCreatedNotification(Guid.NewGuid().ToString());

            var gameServiceMock = new Mock<IGameService>();

            var fixture = new Fixture();
            var battleships = fixture.CreateMany<Battleship>(4).ToList();
            
            var battleshipServiceMock = new Mock<IBattleshipService>();
            battleshipServiceMock
                .Setup(x => x.GetAll(notification.GameId))
                .Returns(battleships);
            
            //when
            var processor =
                new BattleshipCreatedNotificationProcessor<BattleshipCreatedNotification>(battleshipServiceMock.Object, gameServiceMock.Object);
            
            processor.Process(notification);
            
            //then
            gameServiceMock
                .Verify(x => x.TrySetNewStatus(new BattleInProgressStatus(notification.GameId, battleships.Count)), Times.Once);
        }
    }
}