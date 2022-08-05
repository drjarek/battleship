using System.Linq;
using AutoFixture;
using GameEngine.Application.GameApp.Notification;
using GameEngine.Application.PlayerApp.Notification;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;

namespace GameEngine.Tests.Application.GameApp.Notification
{
    public class PlayerCreatedNotificationProcessorTests
    {
        [Fact]
        public void Should_update_game_status_if_all_players_have_been_created()
        {
            //given
            var fixture = new Fixture();

            var notification = new PlayerCreatedNotification(fixture.Create<Player>());

            
            var players = fixture.CreateMany<Player>(2).ToList();

            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock
                .Setup(x => x.GetAll(notification.Player.GameId))
                .Returns(players);

            var gameServiceMock = new Mock<IGameService>();

            //when
            var processor = new PlayerCreatedNotificationProcessor(playerServiceMock.Object, gameServiceMock.Object);
            processor.Process(notification);

            //then
            gameServiceMock
                .Verify(x => x.TrySetNewStatus(new SetupBattleshipStatus(notification.Player.GameId, players.Count)), Times.Once);
        }
    }
}