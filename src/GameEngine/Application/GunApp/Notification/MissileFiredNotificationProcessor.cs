using System.Linq;
using GameEngine.Application.GunApp.Command;
using GameEngine.Domain;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;

namespace GameEngine.Application.GunApp.Notification
{
    public class MissileFiredNotificationProcessor : INotificationProcessor<MissileFiredNotification>
    {
        private readonly ICommandHandler<FireMissileWithRandomCoordinatesCommand> _commandHandler;
        private readonly IPlayerService _playerService;
        private readonly IGameService _gameService;
        private readonly IIdGenerator _idGenerator;

        public MissileFiredNotificationProcessor(
            ICommandHandler<FireMissileWithRandomCoordinatesCommand> commandHandler,
            IPlayerService playerService,
            IGameService gameService,
            IIdGenerator idGenerator)
        {
            _commandHandler = commandHandler;
            _playerService = playerService;
            _gameService = gameService;
            _idGenerator = idGenerator;
        }
        
        public void Process(MissileFiredNotification notification)
        {
            var allPlayers = _playerService.GetAll(notification.GameId).ToList();
            var player = allPlayers.FirstOrDefault(x => x.Id == notification.PlayerId);
            var gameSettings = _gameService.Get(notification.GameId);
            
            if (player == null || player.Type == PlayerType.Cpu || gameSettings.GameStatus != GameStatus.BattleInProgress)
            {
                return;
            }
            
            var cpu = allPlayers.FirstOrDefault(x => x.Id != notification.PlayerId && x.Type == PlayerType.Cpu);
            if (cpu == null)
            {
                return;
            }
            
            _commandHandler.Handle(new FireMissileWithRandomCoordinatesCommand(_idGenerator.New(), notification.GameId, cpu.Id));
        }
    }
}