using System.Linq;
using GameEngine.Application.BattleshipApp.Notification;
using GameEngine.Domain.Service;

namespace GameEngine.Application.GameApp.Notification
{
    public class BattleshipCreatedNotificationProcessor<T> : INotificationProcessor<T>
        where T : BattleshipCreatedNotification
    {
        private readonly IBattleshipService _battleshipService;
        private readonly IGameService _gameService;

        public BattleshipCreatedNotificationProcessor(IBattleshipService battleshipService, IGameService gameService)
        {
            _battleshipService = battleshipService;
            _gameService = gameService;
        }

        public void Process(T notification)
        {
            var battleships = _battleshipService.GetAll(notification.GameId);

            var status = new BattleInProgressStatus(notification.GameId, battleships.Count());

            _gameService.TrySetNewStatus(status);
        }
    }
}