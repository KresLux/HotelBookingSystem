using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsManager
{
    public class Booking
    {
        private static int _nextId = 0;
        private List<INotification> _notifications = new List<INotification>();
        public int BookingId { get; }
        public IRoom Room { get; }
        public string GuestName { get; }
        public string GuestEmail { get; }
        public DateTime CheckIn { get; }
        public DateTime CheckOut { get; }
        public decimal TotalPrice { get; }
        public BookingStatus Status { get; private set; }

        public int Nights => (CheckOut.Date - CheckIn.Date).Days;

        public Booking(IRoom room, string guestName, string guestEmail,
            DateTime checkIn, DateTime checkOut, IPriceStrategy priceStrategy)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            if (priceStrategy == null)
            {
                throw new ArgumentNullException(nameof(priceStrategy));
            }

            if (string.IsNullOrWhiteSpace(guestName))
            {
                throw new ArgumentException("Имя гостя не может быть пустым.", nameof(guestName));
            }

            if (string.IsNullOrWhiteSpace(guestEmail))
            {
                throw new ArgumentException("Email гостя не может быть пустым.", nameof(guestEmail));
            }

            if (checkOut.Date <= checkIn.Date)
            {
                throw new ArgumentException("Дата выезда должна быть позже даты заезда.", nameof(checkOut));
            }

            Room = room;
            GuestName = guestName;
            GuestEmail = guestEmail;
            CheckIn = checkIn;
            CheckOut = checkOut;
            BookingId = ++_nextId;
            Status = BookingStatus.Pending;

            decimal priceByStrategy = priceStrategy.CalculatePrice(room.BasePrice, Nights, CheckIn);
            TotalPrice = DiscountCalculator.ApplyDiscount(priceByStrategy, Nights);
        }

        
        public void Subscribe(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (!_notifications.Contains(notification))
            {
                _notifications.Add(notification);
            }
        }

        public void Unsubscribe(INotification notification)
        {
            _notifications.Remove(notification);
        }
        
        public void Confirm()
        {
            if (Status != BookingStatus.Pending)
            {
                throw new InvalidOperationException($"Нельзя подтвердить бронь в статусе {Status}.");
            }

            Status = BookingStatus.Confirmed;
            Notify($"Бронь #{BookingId} подтверждена: {Room.GetDescription()}, " +
                   $"{CheckIn:dd.MM.yyyy}–{CheckOut:dd.MM.yyyy}, сумма {TotalPrice} руб.");
        }

        public void Cancel()
        {
            if (Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("Бронь уже отменена.");
            }

            Status = BookingStatus.Cancelled;
            Notify($"Бронь #{BookingId} отменена.");
        }

        private void Notify(string message)
        {
            foreach (var notification in _notifications)
            {
                notification.Send(message, GuestEmail);
            }
        }
    }
}
