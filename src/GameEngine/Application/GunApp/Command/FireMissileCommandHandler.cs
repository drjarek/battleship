using GameEngine.Application.GunApp.Notification;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;

namespace GameEngine.Application.GunApp.Command
{
    public class FireMissileCommandHandler : ICommandHandler<FireMissileCommand>
    {
        private readonly IBattleshipGunService _battleshipGunService;
        private readonly INotificationPublisher<MissileFiredNotification> _publisher;

        public FireMissileCommandHandler(IBattleshipGunService battleshipGunService, INotificationPublisher<MissileFiredNotification> publisher)
        {
            _battleshipGunService = battleshipGunService;
            _publisher = publisher;
        }
        
        public void Handle(FireMissileCommand missileCommand)
        {
            var missile = new Missile
            {
                Id = missileCommand.Id,
                GameId = missileCommand.GameId,
                PlayerId = missileCommand.PlayerId,
                Row = missileCommand.Row,
                Column = missileCommand.Column
            };

            _battleshipGunService.Fire(missile);

            _publisher.Publish(new MissileFiredNotification(
                missileCommand.GameId,
                missileCommand.PlayerId,
                missileCommand.Row,
                missileCommand.Column
                )
            );
        }
    }
}