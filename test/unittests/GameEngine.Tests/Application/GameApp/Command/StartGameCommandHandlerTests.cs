using System;
using System.Collections.Generic;
using GameEngine.Application.GameApp.Command;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;
using Moq;
using Xunit;

namespace GameEngine.Tests.Application.GameApp.Command
{
    public class StartGameCommandHandlerTests
    {
        [Fact]
        public void Should_handle_command()
        {
            var gameServiceMock = new Mock<IGameService>();

            var handler = new StartGameCommandHandler(gameServiceMock.Object);
            handler.Handle(new StartGameCommand(
                Guid.NewGuid().ToString(),
                10,
                10,
                new List<int>{3, 4},
                2
                ));

            gameServiceMock.Verify(x => x.Create(It.IsAny<GameSettings>()), Times.Once);
        }
    }
}