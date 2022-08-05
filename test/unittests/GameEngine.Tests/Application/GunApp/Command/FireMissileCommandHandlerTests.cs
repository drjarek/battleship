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
    public class FireMissileCommandHandlerTests
    {
        [Fact]
        public void Should_handle_command()
        {
            //given
            var battleshipGunServiceMock = new Mock<IBattleshipGunService>();
            var publisher = new Mock<INotificationPublisher<MissileFiredNotification>>();

            var handler = new FireMissileCommandHandler(battleshipGunServiceMock.Object, publisher.Object);
            
            //when
            handler.Handle(new FireMissileCommand(NewId(),NewId(), NewId(), 3, 4));
            
            //then
            battleshipGunServiceMock.Verify(x => x.Fire(It.IsAny<Missile>()));
        }

        private static string NewId() => new IdGenerator().New();
    }
}