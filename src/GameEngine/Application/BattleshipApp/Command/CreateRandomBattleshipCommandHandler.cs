using GameEngine.Application.BattleshipApp.Notification;
using GameEngine.Domain.Service;

namespace GameEngine.Application.BattleshipApp.Command
{
    public class CreateRandomBattleshipCommandHandler : ICommandHandler<CreateRandomBattleshipCommand>
    {
        private readonly IBattleshipService _battleshipService;
        private readonly INotificationPublisher<BattleshipCreatedNotification> _notificationPublisher;

        public CreateRandomBattleshipCommandHandler(IBattleshipService battleshipService,
            INotificationPublisher<BattleshipCreatedNotification> notificationPublisher)
        {
            _battleshipService = battleshipService;
            _notificationPublisher = notificationPublisher;
        }
            
        public void Handle(CreateRandomBattleshipCommand command)
        {
            _battleshipService.Create(command.Size, command.GameId, command.PlayerId);

            _notificationPublisher.Publish(new BattleshipCreatedNotification(command.GameId));
        }
    }
}