namespace GameEngine.Application.BattleshipApp.Command
{
    public record CreateRandomBattleshipCommand(
        string GameId,
        int Size,
        string PlayerId
        ) : ICommand;
}