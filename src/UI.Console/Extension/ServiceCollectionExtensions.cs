using Microsoft.Extensions.DependencyInjection;

namespace UI.Console.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUiConsole(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConsoleManager, ConsoleManager>();
            serviceCollection.AddTransient<ITable, Table>();
            serviceCollection.AddTransient<ITableWriter, TableWriter>();
        }
    }
}