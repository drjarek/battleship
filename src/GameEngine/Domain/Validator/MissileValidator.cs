using System.Collections.Generic;
using System.Linq;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Validator
{
    public class MissileValidator : AbstractValidator<Missile>
    {
        private readonly GameSettings _gameSettings;
        private readonly List<Missile> _missiles;

        public MissileValidator(GameSettings gameSettings, List<Missile> missiles)
        {
            _gameSettings = gameSettings;
            _missiles = missiles;
        }

        protected override bool IsValid(Missile model)
        {
            return IsGameStatusValid() && AreCoordinatesValid(model);
        }

        private bool IsGameStatusValid()
        {
            return _gameSettings.GameStatus == GameStatus.BattleInProgress;
        }

        private bool AreCoordinatesValid(Missile missile)
        {
            var isRowValid = missile.Row >= 0 && missile.Row < _gameSettings.MaxRows;
            var isColumnValid = missile.Column >= 0 && missile.Column < _gameSettings.MaxColumns;
            var isNotDuplicated = _missiles.All(x => x.Row != missile.Row || x.Column != missile.Column);

            return isRowValid && isColumnValid && isNotDuplicated;
        }
    }
}

