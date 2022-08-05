using System;
using GameEngine.Application;
using GameEngine.Application.PlayerApp.Command;
using GameEngine.Application.PlayerApp.Notification;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;

namespace GameEngine.Tests.Application.PlayerApp.Command
{
    public class CreatePlayerCommandHandlerTests
    {
        [Fact]
        public void Should_create_player()
        {
            //given
            var playerServiceMock = new Mock<IPlayerService>();
            var publisherMock = new Mock<INotificationPublisher<PlayerCreatedNotification>>();
            
            //given
            var handler = new CreatePlayerCommandHandler(playerServiceMock.Object, publisherMock.Object);
            handler.Handle(new CreatePlayerCommand(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                PlayerType.Cpu)
            );
            
            //then
            playerServiceMock.Verify(x => x.Create(It.IsAny<Player>()), Times.Once);
        }
        
        [Fact]
        public void Should_publish_notification()
        {
            //given
            var playerServiceMock = new Mock<IPlayerService>();
            var publisherMock = new Mock<INotificationPublisher<PlayerCreatedNotification>>();
            
            //given
            var handler = new CreatePlayerCommandHandler(playerServiceMock.Object, publisherMock.Object);
            handler.Handle(new CreatePlayerCommand(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                PlayerType.Cpu)
            );
            
            //then

            publisherMock.Verify(x => x.Publish(It.IsAny<PlayerCreatedNotification>()), Times.Once);
        }
    }
}