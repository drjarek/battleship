using System.Collections.Generic;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Service
{
    public interface IBattleshipService
    {
        public void Create(Battleship battleship);

        public void Create(int size, string gameId, string playerId);

        public bool AddDamagesIfHit(Coordinates coordinates, string gameIdm, string playerId);

        public IEnumerable<Battleship> GetAll(string gameId);

        public IEnumerable<Battleship> GetAllEnemyBattleships(string gameId, string playerId);
        
        public IEnumerable<Battleship> GetAllPlayerBattleships(string gameId, string playerId);
    }
}