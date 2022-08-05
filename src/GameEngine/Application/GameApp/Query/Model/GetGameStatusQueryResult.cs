using GameEngine.Domain.Repository.Model;

namespace GameEngine.Application.GameApp.Query.Model
{
    public record GetGameStatusQueryResult(GameStatus GameStatus, PlayerStatusDto? HumanStatus, PlayerStatusDto? CpuStatus, string? WinnerId);
}