using System.Collections.Generic;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Repository
{
    public interface IMissileRepository
    {
        public List<Missile> GetAllPlayerShots(string gameId, string playerId);

        public Missile Create(Missile model);
        
        public Missile? Get(string gameId, string id);
    }
}

