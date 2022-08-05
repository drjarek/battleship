using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Service
{
    public interface IGameService
    {
        public GameSettings Create(GameSettings gameSettings);
        
        public void Update(GameSettings gameSettings);

        GameSettings Get(string gameId);

        public bool TrySetNewStatus(BattleInProgressStatus status);
        
        public bool TrySetNewStatus(GameFinishedStatus status);
        
        public bool TrySetNewStatus(SetupBattleshipStatus status);
    }
}