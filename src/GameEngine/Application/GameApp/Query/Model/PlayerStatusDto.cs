using System.Collections.Generic;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Application.GameApp.Query.Model
{
    public record PlayerStatusDto(string Id, IEnumerable<BattleshipDto> Battleships,
        IEnumerable<Coordinates> FiredMissiles);
}