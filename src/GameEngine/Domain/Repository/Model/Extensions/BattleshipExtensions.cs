using System.Collections.Generic;

namespace GameEngine.Domain.Repository.Model.Extensions
{
    public static class BattleshipExtensions
    {
        public static int Size(this Battleship battleship)
        {
            var start = battleship.Start;
            var end = battleship.End;

            if (start.Row == end.Row)
            {
                return end.Column - start.Column + 1;
            }

            return end.Row - start.Row + 1;
        }
    
        public static IEnumerable<Coordinates> GetPositions(this Battleship battleship)
        {
            var positions = new List<Coordinates>();
            if (battleship.Start.Column == battleship.End.Column)
            {
                for (var row = battleship.Start.Row; row <= battleship.End.Row; row++)
                {
                    positions.Add(battleship.Start with { Row = row });
                }
            }
            else
            {
                for (var column = battleship.Start.Column; column <= battleship.End.Column; column++)
                {
                    positions.Add(battleship.Start with { Column = column });
                }
            }

            return positions;
        }
    }}

