using System.Collections.Generic;

namespace GameEngine.Domain.Repository.Model
{
    public class Battleship : BaseModel
    {
        public Coordinates Start { get; init; }
    
        public Coordinates End { get; init; }

        public BattleshipStatus Status { get; set; } = BattleshipStatus.Healthy;
    
        public string PlayerId { get; init; }

        public List<Coordinates> Damages { get; } = new();
    }
}

