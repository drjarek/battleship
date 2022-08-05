namespace GameEngine.Application
{
    public interface IQueryHandler<in TIn, out TOut> where TIn : IQuery
    {
        public TOut Handle(TIn query);
    }
}