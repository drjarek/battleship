using GameEngine.Domain.Repository.Model;

namespace GameEngine.Application.PlayerApp.Notification
{
    public record PlayerCreatedNotification(Player Player) : INotification;
}