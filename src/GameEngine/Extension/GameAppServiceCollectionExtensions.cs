using GameEngine.Application;
using GameEngine.Application.BattleshipApp.Notification;
using GameEngine.Application.GameApp.Command;
using GameEngine.Application.GameApp.Notification;
using GameEngine.Application.GameApp.Query;
using GameEngine.Application.GameApp.Query.Model;
using GameEngine.Application.GunApp.Notification;
using GameEngine.Application.PlayerApp.Notification;
using Microsoft.Extensions.DependencyInjection;
using PlayerCreatedNotificationProcessor = GameEngine.Application.GameApp.Notification.PlayerCreatedNotificationProcessor;


namespace GameEngine.Extension
{
    public static class GameAppServiceCollectionExtensions
    {
        public static void AddGameApp(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICommandHandler<StartGameCommand>, StartGameCommandHandler>();
            serviceCollection.AddTransient<INotificationProcessor<BattleshipCreatedNotification>, BattleshipCreatedNotificationProcessor<BattleshipCreatedNotification>>();
            serviceCollection.AddTransient<INotificationProcessor<MissileFiredNotification>, Application.GameApp.Notification.MissileFiredNotificationProcessor<MissileFiredNotification>>();
            serviceCollection.AddTransient<INotificationProcessor<MissileFiredWithRandomCoordinatesNotification>, Application.GameApp.Notification.MissileFiredNotificationProcessor<MissileFiredWithRandomCoordinatesNotification>>();
            serviceCollection.AddTransient<INotificationProcessor<PlayerCreatedNotification>, PlayerCreatedNotificationProcessor>();
            serviceCollection.AddTransient<IQueryHandler<GetGameStatusQuery, GetGameStatusQueryResult>, GetGameStatusQueryHandler>();
        }
    }
}