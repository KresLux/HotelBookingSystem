namespace HotelsManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== СИСТЕМА БРОНИРОВАНИЯ ОТЕЛЯ ===\n");

            // Инициализация менеджера уведомлений
            var notificationManager = new NotificationManager();
            notificationManager.AddNotification(new EmailNotification());
            notificationManager.AddNotification(new SMSNotification());
            notificationManager.AddNotification(new PushNotification());

            // Инициализация менеджера отеля
            var hotelManager = new HotelManager(notificationManager);

            // Запуск тестов
            RunTests(hotelManager, notificationManager);

            Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ЗАВЕРШЕНО ===");
        }

        static void RunTests(HotelManager hotelManager, NotificationManager notificationManager)
        {
            Test1_BasicRoomOperations(hotelManager);
            Test2_PriceStrategies();
            Test3_NotificationSystem(notificationManager);
            Test4_BookingSystem(hotelManager);
            Test5_HotelStatistics(hotelManager);
        }

        static void Test1_BasicRoomOperations(HotelManager hotelManager)
        {
            Console.WriteLine("--- ТЕСТ 1: Базовые операции с номерами ---");

            try
            {
                RoomFactory factory = new RoomFactory();
                var familyRoom = factory.Create(RoomType.Family,"401");
                var standardRoom = new StandardRoom("101");
                var deluxeRoom = new DeluxeRoom("201");
                var suiteRoom = new Suite("301");

                Console.WriteLine($"Создан номер: {standardRoom.GetDescription()}");
                Console.WriteLine($"Создан номер: {deluxeRoom.GetDescription()}");
                Console.WriteLine($"Создан номер: {suiteRoom.GetDescription()}");
                
                hotelManager.AddRoom(standardRoom);
                hotelManager.AddRoom(deluxeRoom);
                hotelManager.AddRoom(suiteRoom);
                hotelManager.AddRoom(familyRoom);


                Console.WriteLine("✅ Базовые операции с номерами выполнены успешно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        static void Test2_PriceStrategies()
        {
            Console.WriteLine("\n--- ТЕСТ 2: Стратегии ценообразования ---");

            try
            {
                var basePrice = 3000m;
                var days = 3;
                var checkInDate = new DateTime(2024, 12, 25); // Праздничные даты

                var regularStrategy = new RegularPriceStrategy();
                var weekendStrategy = new WeekendPriceStrategy();
                var seasonalStrategy = new SeasonalPriceStrategy();

                Console.WriteLine($"Базовая цена: {basePrice} руб./ночь");
                Console.WriteLine($"Период: {days} ночей");
                Console.WriteLine($"Заезд: {checkInDate:dd.MM.yyyy}");
                
                
                Console.WriteLine($"Обычный тариф: {regularStrategy.CalculatePrice(basePrice, days, checkInDate)} руб.");
                Console.WriteLine($"Выходной тариф: {weekendStrategy.CalculatePrice(basePrice, days, checkInDate)} руб.");
                Console.WriteLine($"Сезонный тариф: {seasonalStrategy.CalculatePrice(basePrice, days, checkInDate)} руб.");
                

                // Console.WriteLine("⚠️  Реализуйте методы CalculatePrice в стратегиях");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        static void Test3_NotificationSystem(NotificationManager notificationManager)
        {
            Console.WriteLine("\n--- ТЕСТ 3: Система уведомлений ---");

            try
            {
                var testMessage = "Тестовое уведомление от системы бронирования";
                var testRecipient = "client@example.com";

                Console.WriteLine("Отправка тестовых уведомлений...");
                notificationManager.Broadcast(testMessage, testRecipient);

                Console.WriteLine("✅ Система уведомлений работает");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        static void Test4_BookingSystem(HotelManager hotelManager)
        {
            Console.WriteLine("\n--- ТЕСТ 4: Система бронирования ---");

            try
            {
                var checkIn = DateTime.Now.AddDays(7);
                var checkOut = DateTime.Now.AddDays(10);

                // Поиск доступных номеров
                var availableRooms = hotelManager.FindAvailableRooms();
                Console.WriteLine($"Найдено доступных номеров: {availableRooms.Count}");

                if (availableRooms.Count > 0)
                {
                    var booking = hotelManager.CreateBooking(
                        availableRooms[0].RoomNumber,
                        "Иван Петров",
                        "ivan.petrov@mail.ru",
                        checkIn,
                        checkOut,
                        new RegularPriceStrategy()
                    );

                    Console.WriteLine($"Создано бронирование #{booking.BookingId}");
                    Console.WriteLine($"Гость: {booking.GuestName}");
                    Console.WriteLine($"Стоимость: {booking.TotalPrice} руб.");

                    // Подтверждение бронирования
                    booking.Confirm();
                    

                    // Console.WriteLine("⚠️  Реализуйте систему бронирования");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        static void Test5_HotelStatistics(HotelManager hotelManager)
        {
            Console.WriteLine("\n--- ТЕСТ 5: Статистика отеля ---");

            try
            {
                hotelManager.ShowHotelStatus();

                var statistics = hotelManager.GetRoomStatistics();
                Console.WriteLine("\nСтатистика по типам номеров:");
                foreach (var stat in statistics)
                {
                    Console.WriteLine($"  {stat.Key}: {stat.Value} номеров");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }
    }
}
