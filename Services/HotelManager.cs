using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsManager
{
    public class HotelManager
    {
        private readonly List<IRoom> _rooms = new List<IRoom>();
        private readonly List<Booking> _bookings = new List<Booking>();
        private readonly NotificationManager _notificationManager;
        private readonly RoomFactory _roomFactory = new RoomFactory();

        public HotelManager(NotificationManager notificationManager)
        {
            if (notificationManager == null)
            {
                throw new ArgumentNullException(nameof(notificationManager));
            }

            _notificationManager = notificationManager;
        }

        public void AddRoom(IRoom room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            if (_rooms.Any(r => r.RoomNumber == room.RoomNumber))
            {
                throw new InvalidOperationException($"Номер {room.RoomNumber} уже есть в отеле.");
            }

            _rooms.Add(room);
        }

        public void AddRoom(RoomType type, string roomNumber)
        {
            AddRoom(_roomFactory.Create(type, roomNumber));
        }

        public List<IRoom> FindAvailableRooms()
        {
            return _rooms.Where(room => room.IsAvailable).ToList();
        }

        public List<IRoom> FindAvailableRooms(RoomType roomType)
        {
            return _rooms.Where(room => room.IsAvailable && room.Type == roomType).ToList();
        }

        public List<IRoom> FindAvailableRooms(DateTime checkIn, DateTime checkOut,
            RoomType? type = null,
            int? minCapacity = null,
            decimal? maxPricePerNight = null)
        {
            ValidateDates(checkIn, checkOut);

            return _rooms
                .Where(room => room.IsAvailable)
                .Where(room => type == null || room.Type == type)
                .Where(room => minCapacity == null || room.Capacity >= minCapacity)
                .Where(room => maxPricePerNight == null || room.BasePrice <= maxPricePerNight)
                .Where(room => IsRoomFree(room, checkIn, checkOut))
                .ToList();
        }


        // TODO: Реализовать методы бронирования
        public Booking CreateBooking(string roomNumber, string guestName, string guestEmail,
            DateTime checkIn, DateTime checkOut, IPriceStrategy priceStrategy)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (room == null)
            {
                throw new ArgumentException($"Номер {roomNumber} не найден.", nameof(roomNumber));
            }

            ValidateDates(checkIn, checkOut);

            if (!room.IsAvailable || !IsRoomFree(room, checkIn, checkOut))
            {
                throw new InvalidOperationException(
                    $"Номер {roomNumber} недоступен на {checkIn:dd.MM.yyyy}–{checkOut:dd.MM.yyyy}.");
            }

            var booking = new Booking(room, guestName, guestEmail, checkIn, checkOut, priceStrategy);

            foreach (var notification in _notificationManager.GetAll())
            {
                booking.Subscribe(notification);
            }
            
            _bookings.Add(booking);

            _notificationManager.Broadcast(
                $"Бронь #{booking.BookingId} создана: {room.GetDescription()}, " +
                $"{checkIn:dd.MM.yyyy}–{checkOut:dd.MM.yyyy}, сумма {booking.TotalPrice} руб.",
                guestEmail);

            return booking;
        }

        public void ShowHotelStatus()
        {
            var today = DateTime.Today;
            int total = _rooms.Count;
            int freeToday = FindAvailableRooms(today, today.AddDays(1)).Count;
            int activeBookings = _bookings.Count(b => b.Status != BookingStatus.Cancelled);

            Console.WriteLine("\n=== СТАТУС ОТЕЛЯ ===");
            Console.WriteLine($"Всего номеров: {total}");
            Console.WriteLine($"Свободно сегодня: {freeToday}");
            Console.WriteLine($"Активных броней: {activeBookings}");
        }

        public Dictionary<RoomType, int> GetRoomStatistics()
        {
            var statistics = Enum.GetValues<RoomType>().ToDictionary(type => type, type => 0);

            foreach (var room in _rooms)
            {
                statistics[room.Type]++;
            }
            
            return statistics;
        }

        private bool IsRoomFree(IRoom room, DateTime checkIn, DateTime checkOut)
        {
            return !_bookings.Any(b =>
                b.Room == room
                && b.Status != BookingStatus.Cancelled
                && checkIn.Date < b.CheckOut.Date
                && b.CheckIn.Date < checkOut.Date);
        }

        private static void ValidateDates(DateTime checkIn, DateTime checkOut)
        {
            if (checkOut.Date <= checkIn.Date)
            {
                throw new ArgumentException("Дата выезда должна быть позже даты заезда.", nameof(checkOut));
            }
        }
    }
}