using System.Collections.Generic;
using System.Linq;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Infrastructure.InMemoryRepository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IDictionary<string, List<Player>> _database = new Dictionary<string, List<Player>>();

        public Player? Get(string gameId, string id)
        {
            var all = GetAll(gameId);
            return all.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Player> GetAll(string gameId)
        {
            return _database.ContainsKey(gameId) ? _database[gameId] : new List<Player>();
        }

        public void Create(Player model)
        {
            var allPlayers = GetAll(model.GameId).ToList();

            allPlayers.Add(model);
        
            _database[model.GameId] = allPlayers;
        }
    }
}

