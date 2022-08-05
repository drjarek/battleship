using GameEngine.Application;
using GameEngine.Application.GunApp.Command;
using GameEngine.Application.GunApp.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace GameEngine.Extension
{
    public static class GunAppServiceCollectionExtensions
    {
        public static void AddGunApp(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICommandHandler<FireMissileCommand>, FireMissileCommandHandler>();
            serviceCollection.AddTransient<ICommandHandler<FireMissileWithRandomCoordinatesCommand>, FireMissileWithRandomCoordinatesCommandHandler>();
            serviceCollection.AddTransient<INotificationProcessor<MissileFiredNotification>, MissileFiredNotificationProcessor>();
            serviceCollection.AddTransient<INotificationPublisher<MissileFiredNotification>, DefaultNotificationPublisher<MissileFiredNotification>>();
            serviceCollection.AddTransient<INotificationPublisher<MissileFiredWithRandomCoordinatesNotification>, DefaultNotificationPublisher<MissileFiredWithRandomCoordinatesNotification>>();
        }
    }
}