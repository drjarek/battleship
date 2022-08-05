using System.Linq;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Repository.Model.Extensions;

namespace GameEngine.Domain.Validator
{
    public class BattleshipCoordinatesValidator
    {
        private readonly GameSettings _gameSettings;

        public BattleshipCoordinatesValidator(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public bool IsValid(Battleship battleship)
        {
            return IsRowFromStartCoordinatesValid(battleship)
                   && IsColumnFromStartCoordinatesValid(battleship)
                   && IsRowFromEndCoordinatesValid(battleship)
                   && IsColumnFromEndCoordinatesValid(battleship)
                   && IsSizeValid(battleship)
                   && IsShapeValid(battleship)
                   && AreEndCoordinatesAreGreaterThanStartCoordinates(battleship);
        }

        private bool IsSizeValid(Battleship battleship)
        {
            return _gameSettings.AllowedBattleshipsSize.Any(size => size == battleship.Size());
        }

        private static bool IsShapeValid(Battleship battleship)
        {
            return battleship.Start.Row == battleship.End.Row || battleship.Start.Column == battleship.End.Column;
        }

        private static bool AreEndCoordinatesAreGreaterThanStartCoordinates(Battleship battleship)
        {
            if (battleship.Start.Row == battleship.End.Row)
            {
                return battleship.End.Column > battleship.Start.Column;
            }

            if(battleship.Start.Column == battleship.End.Column)
            {
                return battleship.End.Row > battleship.Start.Row;
            }

            return false;
        }

        private bool IsRowFromStartCoordinatesValid(Battleship battleship)
        {
            return battleship.Start.Row < _gameSettings.MaxRows && battleship.Start.Row >= 0;
        }

        private bool IsColumnFromStartCoordinatesValid(Battleship battleship)
        {
            return battleship.Start.Column < _gameSettings.MaxColumns && battleship.Start.Column >= 0;
        }

        private bool IsRowFromEndCoordinatesValid(Battleship battleship)
        {
            return battleship.End.Row < _gameSettings.MaxRows && battleship.End.Row >= 0;
        }

        private bool IsColumnFromEndCoordinatesValid(Battleship battleship)
        {
            return battleship.End.Column < _gameSettings.MaxColumns && battleship.End.Column >= 0;
        }
    }
}

