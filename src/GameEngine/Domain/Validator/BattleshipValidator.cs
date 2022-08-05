using System.Collections.Generic;
using System.Linq;
using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Repository.Model.Extensions;

namespace GameEngine.Domain.Validator
{
    public class BattleshipValidator
    {
        private readonly GameSettings _gameSettings;
        private readonly IEnumerable<Battleship> _existingBattleships;

        public BattleshipValidator(GameSettings gameSettings, IEnumerable<Battleship> existingBattleships)
        {
            _gameSettings = gameSettings;
            _existingBattleships = existingBattleships;
        }
        public void ThrowExceptionIfBattleshipIsNotValid(Battleship battleship)
        {
            ThrowExceptionIfGameStatusIsInvalid();
            ThrowExceptionIfCantAddAnotherBattleship();
            ThrowExceptionIfCoordinatesAreInvalid(battleship);
            ThrowExceptionIfCantAddBattleshipsWithThatSize(battleship);
            ThrowExceptionIfPositionIsInvalid(battleship);
        }

        private void ThrowExceptionIfGameStatusIsInvalid()
        {
            var status = _gameSettings.GameStatus;
            if (!IsGameStatusValid(status))
            {
                ThrowValidationException($"Game status is '{status.ToString()}'.");
            }
        }

        private static bool IsGameStatusValid(GameStatus status)
        {
            return status == GameStatus.SetupBattleships;
        }
        
        private void ThrowExceptionIfCantAddAnotherBattleship()
        {
            var max = _gameSettings.AllowedBattleshipsSize.Count;
            var amountOfExistingBattleships = _existingBattleships.Count();
            if (max == amountOfExistingBattleships)
            {
                ThrowValidationException($"You can have '{max}' ships only.");
            }
        }

        private void ThrowExceptionIfCoordinatesAreInvalid(Battleship battleship)
        {
            var battleshipCoordinatesValidator = new BattleshipCoordinatesValidator(_gameSettings);
            if (!battleshipCoordinatesValidator.IsValid(battleship))
            {
                ThrowValidationException("Coordinates are invalid.");
            }
        }

        private void ThrowExceptionIfCantAddBattleshipsWithThatSize(Battleship battleship)
        {
            if (!CanAddBattleshipWithThatSize(battleship))
            {
                ThrowValidationException($"Battleship with size '{battleship.Size()}' already exists.");
            }
        }
        
        private bool CanAddBattleshipWithThatSize(Battleship battleship)
        {
            var stats = new Dictionary<int, int>();
            foreach (var size in _gameSettings.AllowedBattleshipsSize)
            {
                var counter = stats.ContainsKey(size) ? stats[size] : 0;
                
                counter++;
                
                stats[size] = counter;
            }
            
            var battleshipSize = battleship.Size();
            var allowedAmountOfBattleships = stats[battleshipSize];

            var numOfExistingShips = _existingBattleships.Count(existingBattleship => existingBattleship.Size() == battleshipSize);

            return numOfExistingShips < allowedAmountOfBattleships;
        }

        private void ThrowExceptionIfPositionIsInvalid(Battleship battleship)
        {
            var battleshipPositionValidator = new BattleshipPositionValidator(_existingBattleships);
            if (!battleshipPositionValidator.IsValid(battleship))
            {
                ThrowValidationException("Positions overlap.");
            }
        }

        private static void ThrowValidationException(string message)
        {
            throw new ValidationException($"Can't set battleship. {message}");
        }
    }
}

