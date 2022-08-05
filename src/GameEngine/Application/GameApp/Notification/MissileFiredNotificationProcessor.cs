using System.Linq;
using GameEngine.Application.GunApp.Notification;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;

namespace GameEngine.Application.GameApp.Notification
{
    public class MissileFiredNotificationProcessor<T> : INotificationProcessor<T> where T : MissileFiredNotification
    {
        private readonly IBattleshipService _battleshipService;
        private readonly IGameService _gameService;

        public MissileFiredNotificationProcessor(IBattleshipService battleshipService, IGameService gameService)
        {
            _battleshipService = battleshipService;
            _gameService = gameService;
        }

        public int Priority => 2;
        
        public void Process(T notification)
        {
            var battleships = _battleshipService.GetAllEnemyBattleships(notification.GameId, notification.PlayerId);
            var amount = battleships.Count(x => x.Status == BattleshipStatus.Destroyed);

            if (!_gameService.TrySetNewStatus(new GameFinishedStatus(notification.GameId, amount))) return;
            
            var gameSettings =  _gameService.Get(notification.GameId);
            gameSettings.WinnerId = notification.PlayerId;
                
            _gameService.Update(gameSettings);
        }
    }
}