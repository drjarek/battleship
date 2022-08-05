using GameEngine.Domain.Repository.Model;

namespace GameEngine.Application.GameApp.Query.Model
{
    public record BattleshipDto(Coordinates Start, Coordinates End);
}