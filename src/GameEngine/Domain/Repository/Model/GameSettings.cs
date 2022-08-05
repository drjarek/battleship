using System.Collections.Generic;

namespace GameEngine.Domain.Repository.Model
{
    public record GameSettings
    {
        public string Id { get; set; }
        
        public int MaxRows { get; init; }

        public int MaxColumns { get; init; }

        public List<int> AllowedBattleshipsSize { get; init; } = new();

        public GameStatus GameStatus { get; set; }

        public int NumberOfPlayers { get; init; }
        
        public string? WinnerId { get; set; }
    }
}

