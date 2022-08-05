using System.Collections.Generic;
using System.Linq;
using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Repository.Model.Extensions;
using GameEngine.Domain.Validator;

namespace GameEngine.Domain.Service
{
    public class BattleshipService : IBattleshipService
    {
        private readonly IBattleshipRepository _battleshipRepository;
        private readonly IGameService _gameService;
        private readonly IBattleshipGenerator _battleshipGenerator;

        public BattleshipService(
            IBattleshipRepository battleshipRepository,
            IBattleshipGenerator battleshipGenerator,
            IGameService gameService
            ) {
            _battleshipRepository = battleshipRepository;
            _gameService = gameService;
            _battleshipGenerator = battleshipGenerator;
        }
        
        public void Create(Battleship battleship)
        {
            var gameId = battleship.GameId;

            var gameSettings = _gameService.Get(gameId);
            var existingBattleships = _battleshipRepository.GetAllPlayerBattleships(gameId, battleship.PlayerId);
            
            var battleshipValidator = new BattleshipValidator(gameSettings, existingBattleships);
            battleshipValidator.ThrowExceptionIfBattleshipIsNotValid(battleship);

            _battleshipRepository.Create(battleship);
        }

        public void Create(int size, string gameId, string playerId)
        {
            var battleship = _battleshipGenerator.Generate(size, playerId, gameId);
            
            Create(battleship);
        }

        public bool AddDamagesIfHit(Coordinates coordinates, string gameId, string playerId)
        {
            var battleshipDamagesValidator = new BattleshipDamagesValidator(_gameService.Get(gameId));
            battleshipDamagesValidator.ThrowExceptionIfIsInvalid();

            var battleships = _battleshipRepository.GetAllEnemyBattleships(gameId, playerId);
            var battleship = battleships
                .FirstOrDefault(x => x.GetPositions().Any(position => position.Row == coordinates.Row && position.Column == coordinates.Column));
            
            if (battleship == null)
            {
                return false;
            }

            if (battleship.Damages.Any(battleshipDamage => battleshipDamage.Row == coordinates.Row && battleshipDamage.Column == coordinates.Column))
            {
                throw new ValidationException("Can't set damages. Can't duplicate damages");
            }
            
            battleship.Damages.Add(coordinates);
                
            battleship.Status = battleship.Size() == battleship.Damages.Count ? BattleshipStatus.Destroyed : BattleshipStatus.Unhealthy;

            _battleshipRepository.Update(battleship);
            
            return true;
        }

        public IEnumerable<Battleship> GetAll(string gameId, string playerId)
        {
            return _battleshipRepository.GetAllPlayerBattleships(gameId, playerId);
        }

        public IEnumerable<Battleship> GetAll(string gameId)
        {
            return _battleshipRepository.GetAll(gameId);
        }
        
        public IEnumerable<Battleship> GetAllEnemyBattleships(string gameId, string playerId)
        {
            return _battleshipRepository.GetAllEnemyBattleships(gameId, playerId);
        }

        public IEnumerable<Battleship> GetAllPlayerBattleships(string gameId, string playerId)
        {
            return _battleshipRepository.GetAllPlayerBattleships(gameId, playerId);
        }
    }
}

