using System.Collections.Generic;

namespace ConsoleApi.Model
{
    public record GameSettingsResponse
    {
        public string GameId { get; init; }
        
        public int MaxRows { get; init; }
        
        public int MaxColumns { get; init; }
        
        public List<int> AllowedBattleshipsSize { get; init; }
        
        public int NumberOfPlayers { get; init; }
        
        public string PlayerId { get; set; }
        
        public string CpuId { get; set; }
    }
}