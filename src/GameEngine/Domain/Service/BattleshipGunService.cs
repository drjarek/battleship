using System.Collections.Generic;
using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Validator;

namespace GameEngine.Domain.Service
{
    public class BattleshipGunService : IBattleshipGunService
    {
        private readonly IMissileRepository _missileRepository;
        private readonly IGameService _gameService;

        public BattleshipGunService(
            IMissileRepository missileRepository,
            IGameService gameService
        ) {
            _missileRepository = missileRepository;
            _gameService = gameService;
        }
        
        public void Fire(Missile missile)
        {
            ThrowExceptionIfShotIsInvalid(missile);

            _missileRepository.Create(missile);
        }

        public void Fire(MissileWithoutCoordinates missileWithoutCoordinates)
        {
            var gameSettings = _gameService.Get(missileWithoutCoordinates.GameId);

            var missiles = _missileRepository.GetAllPlayerShots(missileWithoutCoordinates.GameId, missileWithoutCoordinates.PlayerId);
            var sg = new ShotGenerator(gameSettings);
            var missile = sg.Generate(missileWithoutCoordinates.PlayerId, missiles);
            missile.Id = missileWithoutCoordinates.Id;
            
            Fire(missile);
        }

        public Missile Get(string gameId, string id)
        {
            var missile = _missileRepository.Get(gameId, id);
            if (missile == null)
            {
                throw new NotFoundException($"Missile for '{id}' id not found");
            }

            return missile;
        }

        public IEnumerable<Missile> GetAllPlayerMissiles(string gameId, string playerId)
        {
            return _missileRepository.GetAllPlayerShots(gameId, playerId);
        }

        private void ThrowExceptionIfShotIsInvalid(Missile missile)
        {
            var gameSettings = _gameService.Get(missile.GameId);
            var missiles = _missileRepository.GetAllPlayerShots(missile.GameId, missile.PlayerId);

            new MissileValidator(gameSettings, missiles).ThrowExceptionIfInvalid(missile);
        }
    }
}

