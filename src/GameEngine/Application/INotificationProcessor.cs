namespace GameEngine.Application
{
    public interface INotificationProcessor<in T> where T: INotification
    {
        public void Process(T notification);

        public int Priority => 100;

    }
}