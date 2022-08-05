using System.Collections.Generic;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Infrastructure.InMemoryRepository
{
    public class GameSettingsRepository : IGameSettingsRepository
    {
        private readonly IDictionary<string, GameSettings> _gameSettingsDatabase = new Dictionary<string, GameSettings>();
        
        public void Create(GameSettings model)
        {
            _gameSettingsDatabase.Add(model.Id, model);
        }

        public void Update(GameSettings model)
        {
            _gameSettingsDatabase[model.Id] = model;
        }
        
        public GameSettings? Get(string id)
        {
            return Exists(id) ? _gameSettingsDatabase[id] : null;
        }

        private bool Exists(string id)
        {
            return _gameSettingsDatabase.ContainsKey(id);
        }
    }
}

