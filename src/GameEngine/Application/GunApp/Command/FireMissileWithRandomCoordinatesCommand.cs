namespace GameEngine.Application.GunApp.Command
{
    public record FireMissileWithRandomCoordinatesCommand(
        string Id,
        string GameId,
        string PlayerId
    ) : ICommand;
}