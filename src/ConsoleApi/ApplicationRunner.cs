using System;
using ConsoleApi.Controller;
using ConsoleApi.Extension;
using GameEngine.Domain.Repository.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UI.Console;
using UI.Console.Extension;

namespace ConsoleApi
{
    public class ApplicationRunner
    {
        private readonly IHost _host;
        
        public ApplicationRunner(string[] args)
        {
            _host = BuildDependencies(args);
        }
        
        private static IHost BuildDependencies(string[] args)
        {
            return  Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    {
                        services.AddConsoleApi();
                        services.AddUiConsole();
                    }
                )
                .Build();
        }

        public void Run()
        {
            var gameController = _host.Services.GetRequiredService<GameController>();
            var gameStatusController = _host.Services.GetRequiredService<GameStatusController>();
            var battleshipController = _host.Services.GetRequiredService<BattleshipController>();
            var tableWriter = _host.Services.GetRequiredService<ITableWriter>();
            var consoleManager = _host.Services.GetRequiredService<IConsoleManager>();

            var gameSettings = gameController.Start();
            
            var playerBoardManager = new PlayerBoardManager(gameSettings.MaxRows, gameSettings.MaxColumns);
            var cpuBoardManager = new CpuBoardManager(gameSettings.MaxRows, gameSettings.MaxColumns);

            var status = gameStatusController.Get(gameSettings.GameId);
            PlayerBoard playerBoard;
            PlayerBoard cpuBoard;
            
            do
            {
                consoleManager.ClearConsole();
                
                playerBoard = playerBoardManager.LoadBoard(status?.HumanStatus?.Battleships, status?.CpuStatus?.FiredMissiles);
                cpuBoard = cpuBoardManager.LoadBoard(status?.CpuStatus?.Battleships, status?.HumanStatus?.FiredMissiles);
                
                consoleManager.WriteLine("CPU");
                tableWriter.Write(playerBoard);
                
                consoleManager.WriteLine(string.Empty);
                
                consoleManager.WriteLine("Player");
                tableWriter.Write(cpuBoard);
                
                consoleManager.WriteLine(string.Empty);
                
                consoleManager.Write("Coordinates:");
                var coordinates = consoleManager.ReadLine();
                
                
                try
                {
                    battleshipController.Fire(coordinates, gameSettings.GameId, gameSettings.PlayerId);
                }
                catch (Exception)
                {
                    // ignored
                }

                status = gameStatusController.Get(gameSettings.GameId);
            } while (status?.GameStatus == GameStatus.BattleInProgress);
            
            consoleManager.ClearConsole();
            
            playerBoard = playerBoardManager.LoadBoard(status?.HumanStatus?.Battleships, status?.CpuStatus?.FiredMissiles);
            cpuBoard = cpuBoardManager.LoadBoard(status?.CpuStatus?.Battleships, status?.HumanStatus?.FiredMissiles);
            
            consoleManager.WriteLine("Player");
            tableWriter.Write(playerBoard);
                
            consoleManager.WriteLine(string.Empty);
                
            consoleManager.WriteLine("CPU");
            tableWriter.Write(cpuBoard);

            consoleManager.WriteLine(string.Empty);
            
            if (gameSettings.PlayerId == status?.WinnerId)
            {
                consoleManager.Write("You win!", Colors.Green);
            }
            else
            {
                consoleManager.Write("You loose!", Colors.Red);
            }

            consoleManager.ReadLine();
        }
    }
}