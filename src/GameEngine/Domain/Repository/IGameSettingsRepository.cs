using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Repository
{
    public interface IGameSettingsRepository
    {
        public GameSettings? Get(string id);

        public void Create(GameSettings model);

        public void Update(GameSettings model);
    }
}

