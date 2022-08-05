using GameEngine.Application.BattleshipApp.Command;
using GameEngine.Application.PlayerApp.Notification;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;

namespace GameEngine.Application.BattleshipApp.Notification
{
    public class PlayerCreatedNotificationProcessor : INotificationProcessor<PlayerCreatedNotification>
    {
        private readonly IGameService _gameService;
        private readonly ICommandHandler<CreateRandomBattleshipCommand> _commandHandler;

        public PlayerCreatedNotificationProcessor(ICommandHandler<CreateRandomBattleshipCommand> commandHandler, IGameService gameService)
        {
            _gameService = gameService;
            _commandHandler = commandHandler;
        }

        public void Process(PlayerCreatedNotification notification)
        {
            var player = notification.Player;
            if (player.Type != PlayerType.Cpu)
            {
                return;
            }

            var gameSettings = _gameService.Get(player.GameId);
            foreach (var size in gameSettings.AllowedBattleshipsSize)
            {
                _commandHandler.Handle(new CreateRandomBattleshipCommand(player.GameId, size, player.Id));
            }
        }
    }
}