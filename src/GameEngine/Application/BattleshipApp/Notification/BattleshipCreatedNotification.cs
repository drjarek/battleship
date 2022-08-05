namespace GameEngine.Application.BattleshipApp.Notification
{
    public record BattleshipCreatedNotification(string GameId) : INotification;
}