using GameEngine.Application.GunApp.Notification;
using GameEngine.Domain;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;

namespace GameEngine.Application.GunApp.Command
{
    public class FireMissileWithRandomCoordinatesCommandHandler : ICommandHandler<FireMissileWithRandomCoordinatesCommand>
    {
        private readonly IBattleshipGunService _battleshipGunService;
        private readonly INotificationPublisher<MissileFiredWithRandomCoordinatesNotification> _publisher;
        private readonly IIdGenerator _idGenerator;

        public FireMissileWithRandomCoordinatesCommandHandler(IBattleshipGunService battleshipGunService, INotificationPublisher<MissileFiredWithRandomCoordinatesNotification> publisher, IIdGenerator idGenerator)
        {
            _battleshipGunService = battleshipGunService;
            _publisher = publisher;
            _idGenerator = idGenerator;
        }
        
        public void Handle(FireMissileWithRandomCoordinatesCommand command)
        {
            var id = _idGenerator.New();
            _battleshipGunService.Fire(new MissileWithoutCoordinates
            {
                Id = id,
                GameId = command.GameId,
                PlayerId = command.PlayerId
            });

            var missile = _battleshipGunService.Get(command.GameId, id);

            _publisher.Publish(new MissileFiredWithRandomCoordinatesNotification(
                    missile.GameId,
                    missile.PlayerId,
                    missile.Row,
                    missile.Column
                )
            );
        }
    }
}