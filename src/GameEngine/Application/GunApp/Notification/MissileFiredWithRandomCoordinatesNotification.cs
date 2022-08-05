namespace GameEngine.Application.GunApp.Notification
{
    public record MissileFiredWithRandomCoordinatesNotification
        (string GameId, string PlayerId, int Row, int Column) : MissileFiredNotification(GameId, PlayerId, Row, Column);
}