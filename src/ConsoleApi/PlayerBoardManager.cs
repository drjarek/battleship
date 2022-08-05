using System.Collections.Generic;
using ConsoleApi.Model;
using GameEngine.Application.GameApp.Query.Model;
using GameEngine.Domain.Repository.Model;
using UI.Console;

namespace ConsoleApi
{
    public class PlayerBoardManager
    {
        private readonly int _rows;
        
        private readonly int _columns;
        
        private PlayerBoard _playerBoard;

        public PlayerBoardManager(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
        }

        public PlayerBoard LoadBoard(IEnumerable<BattleshipDto> battleships, IEnumerable<Coordinates> missiles)
        {
            _playerBoard = new PlayerBoard(_rows, _columns);

            SetBattleships(battleships);

            SetMissiles(missiles);

            return _playerBoard;
        }

        private void SetBattleships(IEnumerable<BattleshipDto> battleships)
        {
            foreach (var battleship in battleships)
            {
                SetBattleship(battleship);
            } 
        }

        private void SetBattleship(BattleshipDto battleship)
        {
            var (coordinates, end) = battleship;
            
            if (coordinates.Row == end.Row)
            {
                for (var column = coordinates.Column; column <= end.Column; column++)
                {
                    _playerBoard.SetValue(coordinates.Row, column, BattleshipMark.NotHit, Colors.Green);
                }
            }
            else
            {
                for (var row = coordinates.Row; row <= end.Row; row++)
                {
                    _playerBoard.SetValue(row, coordinates.Column, BattleshipMark.NotHit, Colors.Green);
                }
            }
        }

        private void SetMissiles(IEnumerable<Coordinates> missiles)
        {
            foreach (var missile in missiles)
            {
                var isCellEmpty = _playerBoard.IsCellEmpty(missile.Row, missile.Column);
                
                var color = isCellEmpty
                    ? Colors.Default
                    : Colors.Red;
                
                var value = isCellEmpty
                    ? BattleshipMark.Missile
                    : BattleshipMark.Hit;
                
                _playerBoard.SetValue(missile.Row, missile.Column, value, color);
            }
        }
    }
}

