using System.Collections.Generic;

namespace GameEngine.Application.GameApp.Command
{
    public record StartGameCommand (
        string Id,
        int MaxRows,
        int MaxColumns,
        List<int> AllowedBattleshipsSize,
        int NumberOfPlayers
        ) : ICommand;
}