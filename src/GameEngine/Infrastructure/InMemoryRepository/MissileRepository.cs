using System.Collections.Generic;
using System.Linq;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Infrastructure.InMemoryRepository
{
    public class MissileRepository : IMissileRepository
    {
        private readonly IDictionary<string, List<Missile>> _database = new Dictionary<string, List<Missile>>();

        public Missile Create(Missile model)
        {
            var shoots = GetAll(model.GameId);
        
            shoots.Add(model);

            _database[model.GameId] = shoots;

            return model;
        }

        private List<Missile> GetAll(string gameId)
        {
            return _database.ContainsKey(gameId) ? _database[gameId] : new List<Missile>();
        }

        public List<Missile> GetAllPlayerShots(string gameId, string playerId)
        {
            var shots = GetAll(gameId);

            return shots.Where(shot => shot.PlayerId == playerId).ToList();
        }

        public Missile? Get(string gameId, string id)
        {
            var all = GetAll(gameId);

            return all.FirstOrDefault(x => x.Id == id);
        }
    }
}

