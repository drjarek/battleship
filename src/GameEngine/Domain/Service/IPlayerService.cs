using System.Collections.Generic;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Service
{
    public interface IPlayerService
    {
        public Player Create(Player player);

        public IEnumerable<Player> GetAll(string gameId);
    }
}