using GameEngine.Domain;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Service;
using GameEngine.Infrastructure.InMemoryRepository;
using Microsoft.Extensions.DependencyInjection;

namespace GameEngine.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGameEngine(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddInfrastructure();
            serviceCollection.AddDomain();
            serviceCollection.AddApplication();
        }

        private static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IGameSettingsRepository, GameSettingsRepository>();
            serviceCollection.AddSingleton<IPlayerRepository, PlayerRepository>();
            serviceCollection.AddSingleton<IBattleshipRepository, BattleshipRepository>();
            serviceCollection.AddSingleton<IMissileRepository, MissileRepository>();
        }

        private static void AddDomain(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IBattleshipGunService, BattleshipGunService>();
            serviceCollection.AddTransient<IBattleshipService, BattleshipService>();
            serviceCollection.AddTransient<IGameService, GameService>();
            serviceCollection.AddTransient<IPlayerService, PlayerService>();
            serviceCollection.AddTransient<IBattleshipGenerator, BattleshipGenerator>();
            serviceCollection.AddTransient<IIdGenerator, IdGenerator>();
        }

        private static void AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddBattleshipApp();
            serviceCollection.AddGameApp();
            serviceCollection.AddGunApp();
            serviceCollection.AddPlayerApp();
        }
    }
}

