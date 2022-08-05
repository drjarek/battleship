using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;

namespace GameEngine.Application.GameApp.Command
{
    public class StartGameCommandHandler : ICommandHandler<StartGameCommand>
    {
        private readonly IGameService _gameService;

        public StartGameCommandHandler(IGameService gameService)
        {
            _gameService = gameService;
        }

        public void Handle(StartGameCommand command)
        {
            _gameService.Create(new GameSettings
            {
                Id = command.Id,
                MaxRows = command.MaxRows,
                MaxColumns = command.MaxColumns,
                AllowedBattleshipsSize = command.AllowedBattleshipsSize,
                GameStatus = GameStatus.SetupPlayers,
                NumberOfPlayers = command.NumberOfPlayers
            });
        }
    }
}