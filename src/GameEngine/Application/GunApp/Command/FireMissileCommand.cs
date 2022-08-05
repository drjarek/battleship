namespace GameEngine.Application.GunApp.Command
{
    public record FireMissileCommand(
        string Id,
        string GameId,
        string PlayerId,
        int Row,
        int Column
    ) : ICommand;
}