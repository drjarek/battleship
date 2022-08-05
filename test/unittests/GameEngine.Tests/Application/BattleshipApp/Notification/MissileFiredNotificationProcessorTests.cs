using AutoFixture;
using GameEngine.Application.BattleshipApp.Notification;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;
using MissileFiredNotification = GameEngine.Application.GunApp.Notification.MissileFiredNotification;

namespace GameEngine.Tests.Application.BattleshipApp.Notification
{
    public class MissileFiredNotificationProcessorTests
    {
        private readonly Fixture _fixture;
        
        public MissileFiredNotificationProcessorTests()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Should_process_notification()
        {
            //given
            var notification = _fixture.Create<MissileFiredNotification>();
            var battleshipServiceMock = new Mock<IBattleshipService>();

            var processor = new MissileFiredNotificationProcessor<MissileFiredNotification>(battleshipServiceMock.Object);
            
            //when
            processor.Process(notification);
            
            //then
            battleshipServiceMock.Verify(x => x.AddDamagesIfHit(It.IsAny<Coordinates>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}