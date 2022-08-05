using System.Collections.Generic;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Service
{
    public interface IBattleshipGunService
    {
        public void Fire(Missile missile);

        public void Fire(MissileWithoutCoordinates missile);
        
        public Missile Get(string gameId, string id);
        
        public IEnumerable<Missile> GetAllPlayerMissiles(string gameId, string playerId);
    }
}