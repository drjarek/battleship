using System.Collections.Generic;
using System.Linq;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Infrastructure.InMemoryRepository
{
    public class BattleshipRepository : IBattleshipRepository
    {
        private IDictionary<string, IDictionary<string, Battleship>> BattleshipsDatabase { get; } =
            new Dictionary<string, IDictionary<string, Battleship>>();
    
        public void Create(Battleship model)
        {
            var battleships = BattleshipsDatabase.ContainsKey(model.GameId)
                ? BattleshipsDatabase[model.GameId]
                : new Dictionary<string, Battleship>();
        
            battleships.Add(model.Id, model);
        
            BattleshipsDatabase[model.GameId] = battleships;
        }

        public void Update(Battleship model)
        {
            BattleshipsDatabase[model.GameId][model.Id] = model;
        }

        public Battleship? Get(string gameId, string id)
        {
            var battleships = GetAll(gameId);
            return battleships.FirstOrDefault(battleship => battleship.Id == id);
        }
        
        public IEnumerable<Battleship> GetAll(string gameId)
        {
            return BattleshipsDatabase.ContainsKey(gameId) ? BattleshipsDatabase[gameId].Values.ToList() : new List<Battleship>();
        }
    
        public IEnumerable<Battleship> GetAllPlayerBattleships(string gameId, string playerId)
        {
            var allBattleships = GetAll(gameId);

            return allBattleships.Where(battleship => battleship.PlayerId == playerId).ToList();
        }
    
        public IEnumerable<Battleship> GetAllEnemyBattleships(string gameId, string playerId)
        {
            var allBattleships = GetAll(gameId);

            return allBattleships.Where(battleship => battleship.PlayerId != playerId).ToList();
        }
    }
}

