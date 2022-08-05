using GameEngine.Application;
using GameEngine.Application.GameApp.Query;
using GameEngine.Application.GameApp.Query.Model;

namespace ConsoleApi.Controller
{
    public class GameStatusController
    {
        private readonly IQueryHandler<GetGameStatusQuery, GetGameStatusQueryResult> _queryHandler;

        public GameStatusController(IQueryHandler<GetGameStatusQuery, GetGameStatusQueryResult> queryHandler)
        {
            _queryHandler = queryHandler;
        }

        public GetGameStatusQueryResult Get(string gameId)
        {
            return _queryHandler.Handle(new GetGameStatusQuery(gameId));
        }
        
    }
}