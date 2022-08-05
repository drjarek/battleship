using AutoFixture;
using GameEngine.Application;
using GameEngine.Application.BattleshipApp.Command;
using GameEngine.Application.BattleshipApp.Notification;
using GameEngine.Domain.Service;
using Moq;
using Xunit;

namespace GameEngine.Tests.Application.BattleshipApp.Command
{
    public class CreateRandomBattleshipCommandHandlerTests
    {
        [Fact]
        public void Should_handle_command()
        {
            //given
            var fixture = new Fixture();

            var command = fixture.Create<CreateRandomBattleshipCommand>();
            
            var battleshipService = new Mock<IBattleshipService>();
            var publisherMock = new Mock<INotificationPublisher<BattleshipCreatedNotification>>();

            var handler = new CreateRandomBattleshipCommandHandler(battleshipService.Object, publisherMock.Object);
            
            //when
            handler.Handle(command);
            
            //then
            battleshipService.Verify(x => x.Create(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        
        [Fact]
        public void Should_publish_notification()
        {
            //given
            var fixture = new Fixture();

            var command = fixture.Create<CreateRandomBattleshipCommand>();
            
            var battleshipService = new Mock<IBattleshipService>();
            var publisherMock = new Mock<INotificationPublisher<BattleshipCreatedNotification>>();

            var handler = new CreateRandomBattleshipCommandHandler(battleshipService.Object, publisherMock.Object);
            
            //when
            handler.Handle(command);
            
            //then
            publisherMock.Verify(x => x.Publish(It.IsAny<BattleshipCreatedNotification>()), Times.Once);
        }
    }
}