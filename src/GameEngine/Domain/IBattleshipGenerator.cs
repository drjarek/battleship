using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain
{
    public interface IBattleshipGenerator
    {
        public Battleship Generate(int size, string playerId, string gameId);
    }
}