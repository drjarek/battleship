using System.Collections.Generic;
using ConsoleApi.Model;
using GameEngine.Application;
using GameEngine.Application.BattleshipApp.Command;
using GameEngine.Application.GameApp.Command;
using GameEngine.Application.PlayerApp.Command;
using GameEngine.Domain;
using GameEngine.Domain.Repository.Model;

namespace ConsoleApi.Controller
{
    public class GameController
    {
        private readonly ICommandHandler<StartGameCommand> _startGameCommandHandler;
        private readonly ICommandHandler<CreatePlayerCommand> _createPlayerCommandHandler;
        private readonly ICommandHandler<CreateRandomBattleshipCommand> _createRandomBattleshipCommandHandler;
        private readonly IIdGenerator _idGenerator;

        public GameController(
            ICommandHandler<StartGameCommand> startGameCommandHandler,
            ICommandHandler<CreatePlayerCommand> createPlayerCommandHandler,
            ICommandHandler<CreateRandomBattleshipCommand> createRandomBattleshipCommandHandler,
            IIdGenerator idGenerator
            )
        {
            _startGameCommandHandler = startGameCommandHandler;
            _createPlayerCommandHandler = createPlayerCommandHandler;
            _createRandomBattleshipCommandHandler = createRandomBattleshipCommandHandler;
            _idGenerator = idGenerator;
        }
        
        public GameSettingsResponse Start()
        {
            var gameId = _idGenerator.New();
            var allowedBattleshipSize = new List<int> { 4, 4, 5 };
            
            var startGameCommand = new StartGameCommand(
                gameId,
                10,
                10,
                allowedBattleshipSize,
                2
            );
            
            _startGameCommandHandler.Handle(startGameCommand);

            var createHumanPlayerCommand = new CreatePlayerCommand(
                _idGenerator.New(),
                gameId,
                PlayerType.Human
            );
            _createPlayerCommandHandler.Handle(createHumanPlayerCommand);

            var createCpuPlayerCommand = new CreatePlayerCommand(
                _idGenerator.New(),
                gameId,
                PlayerType.Cpu
            );

            _createPlayerCommandHandler.Handle(createCpuPlayerCommand);

            foreach (var size in allowedBattleshipSize)
            {
                _createRandomBattleshipCommandHandler.Handle(new CreateRandomBattleshipCommand(gameId, size, createHumanPlayerCommand.PlayerId));
            }

            return new GameSettingsResponse
            {
                GameId = gameId,
                MaxRows = startGameCommand.MaxColumns,
                MaxColumns = startGameCommand.MaxColumns,
                AllowedBattleshipsSize = startGameCommand.AllowedBattleshipsSize,
                NumberOfPlayers = startGameCommand.NumberOfPlayers,
                PlayerId = createHumanPlayerCommand.PlayerId,
                CpuId = createCpuPlayerCommand.PlayerId
            };
        }
    }
}