using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Application
{
    public class DefaultNotificationPublisher<T> : INotificationPublisher<T> where T : INotification
    {
        private readonly IEnumerable<INotificationProcessor<T>> _processors;

        public DefaultNotificationPublisher(IEnumerable<INotificationProcessor<T>> processors)
        {
            _processors = processors;
        }
        
        public void Publish(T notification)
        {
            var sortedProcessors = _processors.OrderBy(x => x.Priority);
            foreach (var processor in sortedProcessors)
            {
                processor.Process(notification);
            }
        }
    }
}