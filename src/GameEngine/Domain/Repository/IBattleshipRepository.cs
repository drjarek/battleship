using System.Collections.Generic;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Repository
{
    public interface IBattleshipRepository
    {
        public void Create(Battleship model);
    
        public void Update(Battleship model);
    
        public IEnumerable<Battleship> GetAllPlayerBattleships(string gameId, string playerId);

        public IEnumerable<Battleship> GetAllEnemyBattleships(string gameId, string playerId);
        
        public IEnumerable<Battleship> GetAll(string gameId);

        public Battleship? Get(string gameId, string id);
    }
}

