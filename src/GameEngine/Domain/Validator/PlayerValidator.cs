using System.Collections.Generic;
using System.Linq;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Validator
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        private readonly GameSettings _gameSettings;
        private readonly IEnumerable<Player> _existingPlayers;

        public PlayerValidator(GameSettings gameSettings, IEnumerable<Player> existingPlayers)
        {
            _gameSettings = gameSettings;
            _existingPlayers = existingPlayers;
        }

        protected override bool IsValid(Player player)
        {
            return IsGameStatusValid() && CanAddNewPlayer();
        }

        private bool IsGameStatusValid()
        {
            return _gameSettings.GameStatus == GameStatus.SetupPlayers;
        }

        private bool CanAddNewPlayer()
        {
            return _gameSettings.NumberOfPlayers > _existingPlayers.Count();
        }
    }
}

