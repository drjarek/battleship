using System.Collections.Generic;
using AutoFixture;
using GameEngine.Application;
using GameEngine.Application.GunApp.Notification;
using Moq;
using Xunit;

namespace GameEngine.Tests.Application.GunApp.Notification
{
    public class MissileFiredNotificationPublisherTests
    {
        [Fact]
        public void Should_publish_notification()
        {
            //given
            var fixture = new Fixture();
            var notification = fixture.Create<MissileFiredNotification>();
            
            var notificationProcessor = new Mock<INotificationProcessor<MissileFiredNotification>>();
            
            var notificationProcessors = new List<INotificationProcessor<MissileFiredNotification>>{ notificationProcessor.Object };
            var publisher = new DefaultNotificationPublisher<MissileFiredNotification>(notificationProcessors);
            
            //when
            publisher.Publish(notification);
            
            //then
            notificationProcessor.Verify(x => x.Process(notification), Times.Once);
        }
    }
}