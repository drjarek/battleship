namespace GameEngine.Application
{
    public interface INotificationPublisher<in T> where T: INotification
    {
        public void Publish(T notification);
    }
}