using System.Linq;
using GameEngine.Application.PlayerApp.Notification;
using GameEngine.Domain.Service;

namespace GameEngine.Application.GameApp.Notification
{
    public class PlayerCreatedNotificationProcessor : INotificationProcessor<PlayerCreatedNotification>
    {
        private readonly IPlayerService _playerService;
        private readonly IGameService _gameService;

        public PlayerCreatedNotificationProcessor(IPlayerService playerService, IGameService gameService)
        {
            _playerService = playerService;
            _gameService = gameService;
        }
        
        public int Priority => 1;
        
        public void Process(PlayerCreatedNotification notification)
        {
            var players = _playerService.GetAll(notification.Player.GameId);
            
            _gameService.TrySetNewStatus(new SetupBattleshipStatus(notification.Player.GameId, players.Count()));
        }
    }
}