using AutoFixture;
using GameEngine.Application;
using GameEngine.Application.GunApp.Command;
using GameEngine.Application.GunApp.Notification;
using GameEngine.Domain;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;

namespace GameEngine.Tests.Application.GunApp.Command
{
    public class FireMissileWithRandomCoordinatesCommandHandlerTests
    {
        private readonly Fixture _fixture;
        
        public FireMissileWithRandomCoordinatesCommandHandlerTests()
        {
            _fixture = new Fixture();
        }
        
        [Fact]
        public void Should_handle_command()
        {
            //given
            var missile = _fixture.Create<Missile>();
            
            var battleshipGunServiceMock = new Mock<IBattleshipGunService>();
            battleshipGunServiceMock
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(missile);
            
            var publisher = new Mock<INotificationPublisher<MissileFiredNotification>>();

            var handler = new FireMissileWithRandomCoordinatesCommandHandler(battleshipGunServiceMock.Object, publisher.Object, new IdGenerator());
            
            //when
            handler.Handle(new FireMissileWithRandomCoordinatesCommand(NewId(),NewId(), NewId()));
            
            //then
            battleshipGunServiceMock.Verify(x => x.Fire(It.IsAny<MissileWithoutCoordinates>()));
        }
        
        [Fact]
        public void Should_publish_notification()
        {
            //given
            var missile = _fixture.Create<Missile>();
            
            var battleshipGunServiceMock = new Mock<IBattleshipGunService>();
            battleshipGunServiceMock
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(missile);
            
            var publisher = new Mock<INotificationPublisher<MissileFiredNotification>>();

            var handler = new FireMissileWithRandomCoordinatesCommandHandler(battleshipGunServiceMock.Object, publisher.Object, new IdGenerator());
            
            //when
            handler.Handle(new FireMissileWithRandomCoordinatesCommand(NewId(),NewId(), NewId()));
            
            //then
            publisher.Verify(x => x.Publish(It.IsAny<MissileFiredNotification>()), Times.Once);
        }

        private static string NewId() => new IdGenerator().New();
    }
}