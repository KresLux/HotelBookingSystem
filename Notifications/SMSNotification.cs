using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsManager
{
    public class SMSNotification : INotification
    {
        public NotificationType Type => NotificationType.SMS;

        public bool Send(string message, string recipient)
        {
            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrWhiteSpace(recipient))
            {
                return false;
            }
            Console.WriteLine($"📱 SMS для {recipient}: {message}");
            return true;
        }
    }
}
