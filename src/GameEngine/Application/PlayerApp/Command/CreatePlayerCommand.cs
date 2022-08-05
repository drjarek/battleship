using GameEngine.Domain.Repository.Model;

namespace GameEngine.Application.PlayerApp.Command
{
    public record CreatePlayerCommand(string PlayerId, string GameId, PlayerType PlayerType) : ICommand;
}