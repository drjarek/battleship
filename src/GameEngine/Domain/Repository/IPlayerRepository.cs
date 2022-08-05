using System.Collections.Generic;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Repository
{
    public interface IPlayerRepository
    {
        public Player? Get(string gameId, string id);
        
        public IEnumerable<Player> GetAll(string gameId);
    
        public void Create(Player model);
    }
}

