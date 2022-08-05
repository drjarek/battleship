namespace GameEngine.Application.GunApp.Notification
{
    public record MissileFiredNotification(
        string GameId,
        string PlayerId,
        int Row,
        int Column) : INotification;
}