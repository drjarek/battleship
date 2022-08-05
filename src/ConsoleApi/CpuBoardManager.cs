using System.Collections.Generic;
using ConsoleApi.Model;
using GameEngine.Application.GameApp.Query.Model;
using GameEngine.Domain.Repository.Model;
using UI.Console;

namespace ConsoleApi
{
    public class CpuBoardManager
    {
        private readonly int _rows;

        private readonly int _columns;

        private PlayerBoard _playerBoard;

        public CpuBoardManager(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
        }

        public PlayerBoard LoadBoard(IEnumerable<BattleshipDto> battleships, IEnumerable<Coordinates> missiles)
        {
            _playerBoard = new PlayerBoard(_rows, _columns);

            SetMissiles(missiles);

            SetBattleships(battleships);

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
                    var isEmpty = _playerBoard.IsCellEmpty(coordinates.Row, column);

                    if (isEmpty)
                    {
                        continue;
                    }
                    
                    _playerBoard.SetValue(coordinates.Row, column, BattleshipMark.Hit, Colors.Red);
                }
            }
            else
            {
                for (var row = coordinates.Row; row <= end.Row; row++)
                {
                    var isEmpty = _playerBoard.IsCellEmpty(row, coordinates.Column);

                    if (isEmpty)
                    {
                        continue;
                    }
                    
                    _playerBoard.SetValue(row, coordinates.Column, BattleshipMark.Hit, Colors.Red);
                }
            }
        }

        private void SetMissiles(IEnumerable<Coordinates> missiles)
        {
            foreach (var missile in missiles)
            {
                _playerBoard.SetValue(missile.Row, missile.Column, BattleshipMark.Missile);
            }
        }
    }
}