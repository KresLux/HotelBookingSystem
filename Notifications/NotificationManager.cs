using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsManager
{
    public class NotificationManager
    {
        private List<INotification> _notifications = new List<INotification>();

        public void AddNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (_notifications.Any(n => n.Type == notification.Type))
            {
                return;
            }
            
            _notifications.Add(notification);
        }
        
        public bool RemoveNotification(INotification notification)
        {
            return _notifications.Remove(notification);
        }

        public IReadOnlyList<INotification> GetAll()
        {
            return _notifications;
        }
        public void Broadcast(string message, string recipient)
        {
            foreach (var notification in _notifications)
            {
                notification.Send(message, recipient);
            }
        }
    }
}
