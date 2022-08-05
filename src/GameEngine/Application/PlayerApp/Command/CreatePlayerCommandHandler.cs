using GameEngine.Application.PlayerApp.Notification;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;

namespace GameEngine.Application.PlayerApp.Command
{
    public class CreatePlayerCommandHandler : ICommandHandler<CreatePlayerCommand>
    {
        private readonly IPlayerService _playerService;
        private readonly INotificationPublisher<PlayerCreatedNotification> _publisher;

        public CreatePlayerCommandHandler(IPlayerService playerService, INotificationPublisher<PlayerCreatedNotification> publisher)
        {
            _playerService = playerService;
            _publisher = publisher;
        }
        
        public void Handle(CreatePlayerCommand command)
        {
            var player = _playerService.Create(new Player
            {
                Id = command.PlayerId,
                GameId = command.GameId,
                Type = command.PlayerType
            });
            
            _publisher.Publish(new PlayerCreatedNotification(player));
        }
    }
}