namespace GameEngine.Application.GameApp.Query
{
    public record GetGameStatusQuery(string GameId) : IQuery;
}