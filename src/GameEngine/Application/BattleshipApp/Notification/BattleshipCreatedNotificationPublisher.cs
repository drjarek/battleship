using System.Collections.Generic;

namespace GameEngine.Application.BattleshipApp.Notification
{
    public class BattleshipCreatedNotificationPublisher : DefaultNotificationPublisher<BattleshipCreatedNotification>
    {
        public BattleshipCreatedNotificationPublisher(IEnumerable<INotificationProcessor<BattleshipCreatedNotification>> processors) : base(processors)
        {
        }
    }
}