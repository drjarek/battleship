using ConsoleApi.Controller;
using GameEngine.Domain;
using GameEngine.Extension;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ConsoleApi.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConsoleApi(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddGameEngine();
            
            serviceCollection.TryAddTransient<IIdGenerator, IdGenerator>();
            serviceCollection.TryAddTransient<GameStatusController>();
            serviceCollection.TryAddTransient<BattleshipController>();
            serviceCollection.TryAddTransient<GameController>();
        }
    }
}