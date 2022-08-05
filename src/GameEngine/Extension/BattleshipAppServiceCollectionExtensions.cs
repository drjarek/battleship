using GameEngine.Application;
using GameEngine.Application.BattleshipApp.Command;
using GameEngine.Application.BattleshipApp.Notification;
using GameEngine.Application.GunApp.Notification;
using GameEngine.Application.PlayerApp.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace GameEngine.Extension
{
    public static class BattleshipAppServiceCollectionExtensions
    {
        public static void AddBattleshipApp(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICommandHandler<CreateRandomBattleshipCommand>, CreateRandomBattleshipCommandHandler>();
            serviceCollection.AddTransient<INotificationPublisher<BattleshipCreatedNotification> , DefaultNotificationPublisher<BattleshipCreatedNotification>>();
            serviceCollection.AddTransient<INotificationProcessor<MissileFiredNotification>, MissileFiredNotificationProcessor<MissileFiredNotification>>();
            serviceCollection.AddTransient<INotificationProcessor<MissileFiredWithRandomCoordinatesNotification>, MissileFiredNotificationProcessor<MissileFiredWithRandomCoordinatesNotification>>();
            serviceCollection.AddTransient<INotificationProcessor<PlayerCreatedNotification>, PlayerCreatedNotificationProcessor>();
        }
    }
}