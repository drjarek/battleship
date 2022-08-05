using GameEngine.Application.GunApp.Notification;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;

namespace GameEngine.Application.BattleshipApp.Notification
{
    public class MissileFiredNotificationProcessor<T> : INotificationProcessor<T> where T : MissileFiredNotification
    {
        private readonly IBattleshipService _battleshipService;

        public MissileFiredNotificationProcessor(IBattleshipService battleshipService)
        {
            _battleshipService = battleshipService;
        }

        public int Priority => 1;
        
        public void Process(T notification)
        {
            _battleshipService.AddDamagesIfHit(new Coordinates
                {
                    Row = notification.Row,
                    Column = notification.Column
                },
                notification.GameId,
                notification.PlayerId);
        }
    }
}