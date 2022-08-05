using GameEngine.Application;
using PlayerCreatedGameNotificationProcessor = GameEngine.Application.GameApp.Notification.PlayerCreatedNotificationProcessor;
using PlayerCreatedBattleshipNotificationProcessor = GameEngine.Application.BattleshipApp.Notification.PlayerCreatedNotificationProcessor;
using GameEngine.Application.PlayerApp.Command;
using GameEngine.Application.PlayerApp.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace GameEngine.Extension
{
    public static class PlayerAppServiceCollectionExtensions
    {
        public static void AddPlayerApp(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICommandHandler<CreatePlayerCommand>, CreatePlayerCommandHandler>();
            serviceCollection.AddTransient<INotificationPublisher<PlayerCreatedNotification>, DefaultNotificationPublisher<PlayerCreatedNotification>>();
        }
    }
}