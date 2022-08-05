using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Validator
{
    public class BattleshipDamagesValidator
    {
        private readonly GameSettings _gameSettings;

        public BattleshipDamagesValidator(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public void ThrowExceptionIfIsInvalid()
        {
            if (_gameSettings.GameStatus != GameStatus.BattleInProgress)
            {
                ThrowValidationException($"Game status is '{_gameSettings.GameStatus .ToString()}'.");
            }
        }

        private static void ThrowValidationException(string message)
        {
            throw new ValidationException($"Can't set damages. {message}");
        }
    }
}