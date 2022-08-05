using System.Collections.Generic;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Validator;

namespace GameEngine.Domain.Service
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameService _gameService;

        public PlayerService(IPlayerRepository playerRepository, IGameService gameService)
        {
            _playerRepository = playerRepository;
            _gameService = gameService;
        }

        public Player Create(Player player)
        {
            var gameSettings = _gameService.Get(player.GameId);
            var players = _playerRepository.GetAll(player.GameId);

            new PlayerValidator(gameSettings, players).ThrowExceptionIfInvalid(player);
            _playerRepository.Create(player);

            return player;
        }

        public IEnumerable<Player> GetAll(string gameId)
        {
            return _playerRepository.GetAll(gameId);
        }
    }
}

