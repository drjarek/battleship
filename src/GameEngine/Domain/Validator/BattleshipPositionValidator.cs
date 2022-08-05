using System.Collections.Generic;
using System.Linq;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Repository.Model.Extensions;

namespace GameEngine.Domain.Validator
{
    public class BattleshipPositionValidator
    {
        private readonly IEnumerable<Battleship> _existingBattleships;

        public BattleshipPositionValidator(IEnumerable<Battleship> existingBattleships)
        {
            _existingBattleships = existingBattleships;
        }

        public bool IsValid(Battleship battleship)
        {
            var battleshipPositions = battleship.GetPositions();
            var otherBattleshipsPositions = GetAllBattleshipsPositions(_existingBattleships);

            return battleshipPositions.All(position => !otherBattleshipsPositions.Any(otherPosition => position.Row == otherPosition.Row && position.Column == otherPosition.Column));
        }

        private static List<Coordinates> GetAllBattleshipsPositions(IEnumerable<Battleship> battleships)
        {
            var positions = new List<Coordinates>();
            foreach (var battleship in battleships)
            {
                positions.AddRange(battleship.GetPositions());
            }

            return positions;
        }
    }
}

