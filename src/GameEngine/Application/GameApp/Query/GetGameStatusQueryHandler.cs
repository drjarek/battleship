using System.Collections.Generic;
using System.Linq;
using GameEngine.Application.GameApp.Query.Model;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Service;

namespace GameEngine.Application.GameApp.Query
{
    public class GetGameStatusQueryHandler : IQueryHandler<GetGameStatusQuery, GetGameStatusQueryResult>
    {
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;
        private readonly IBattleshipService _battleshipService;
        private readonly IBattleshipGunService _battleshipGunService;

        public GetGameStatusQueryHandler(
            IGameService gameService,
            IPlayerService playerService,
            IBattleshipService battleshipService,
            IBattleshipGunService battleshipGunService
            )
        {
            _gameService = gameService;
            _playerService = playerService;
            _battleshipService = battleshipService;
            _battleshipGunService = battleshipGunService;
        }
        
        public GetGameStatusQueryResult Handle(GetGameStatusQuery query)
        {
            var gameSettings = _gameService.Get(query.GameId);
            var players = _playerService.GetAll(query.GameId).ToList();
            
            var human = players.FirstOrDefault(x => x.Type == PlayerType.Human);
            var cpu = players.FirstOrDefault(x => x.Type == PlayerType.Cpu);

            PlayerStatusDto? humanStatusDto = null;
            
            if (human != null)
            {
                humanStatusDto = CreatePlayerStatusDto(query.GameId, human);
            }
            
            PlayerStatusDto? cpuStatusDto = null;
            if (cpu != null)
            {
                cpuStatusDto = CreatePlayerStatusDto(query.GameId, cpu);
            }

            return new GetGameStatusQueryResult(gameSettings.GameStatus, humanStatusDto, cpuStatusDto, gameSettings.WinnerId);
        }

        private PlayerStatusDto? CreatePlayerStatusDto(string gameId, Player player)
        {
            var battleships = _battleshipService.GetAllPlayerBattleships(gameId, player.Id).ToList();
            var firedMissiles = _battleshipGunService.GetAllPlayerMissiles(gameId, player.Id).ToList();

            return new PlayerStatusDto(player.Id, ToDto(battleships), ToDto(firedMissiles));
        }

        private static IEnumerable<BattleshipDto> ToDto(IEnumerable<Battleship> battleships)
        {
            return battleships.Select(battleship => new BattleshipDto(battleship.Start, battleship.End)).ToList();
        }
        
        private static IEnumerable<Coordinates> ToDto(IEnumerable<Missile> missiles)
        {
            return missiles.Select(missile => new Coordinates
            {
                Row = missile.Row,
                Column = missile.Column
            }).ToList();
        }
    }
}