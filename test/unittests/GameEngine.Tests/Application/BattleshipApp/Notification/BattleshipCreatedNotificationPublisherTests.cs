using System;
using System.Collections.Generic;
using GameEngine.Application;
using GameEngine.Application.BattleshipApp.Notification;
using Moq;
using Xunit;

namespace GameEngine.Tests.Application.BattleshipApp.Notification
{
    public class BattleshipCreatedNotificationPublisherTests
    {
        [Fact]
        public void Should_publish_notification()
        {
            //given
            var notification = new BattleshipCreatedNotification(Guid.NewGuid().ToString());
            
            var notificationProcessor = new Mock<INotificationProcessor<BattleshipCreatedNotification>>();
            
            var notificationProcessors = new List<INotificationProcessor<BattleshipCreatedNotification>>{ notificationProcessor.Object };
            var publisher = new BattleshipCreatedNotificationPublisher(notificationProcessors);
            
            //when
            publisher.Publish(notification);
            
            //then
            notificationProcessor.Verify(x => x.Process(notification), Times.Once);
        }
    }
}