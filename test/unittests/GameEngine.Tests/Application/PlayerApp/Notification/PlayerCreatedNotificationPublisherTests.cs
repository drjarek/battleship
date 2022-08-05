using System.Collections.Generic;
using GameEngine.Application;
using GameEngine.Application.PlayerApp.Notification;
using GameEngine.Domain.Repository.Model;
using Moq;
using Xunit;

namespace GameEngine.Tests.Application.PlayerApp.Notification
{
    public class PlayerCreatedNotificationPublisherTests
    {
        [Fact]
        public void Should_publish_notification()
        {
            //given
            var notification = new PlayerCreatedNotification(new Player());
            
            var notificationProcessor = new Mock<INotificationProcessor<PlayerCreatedNotification>>();
            
            var notificationProcessors = new List<INotificationProcessor<PlayerCreatedNotification>>{ notificationProcessor.Object };
            var publisher = new DefaultNotificationPublisher<PlayerCreatedNotification>(notificationProcessors);
            
            //when
            publisher.Publish(notification);
            
            //then
            notificationProcessor.Verify(x => x.Process(notification), Times.Once);
        }
    }
}