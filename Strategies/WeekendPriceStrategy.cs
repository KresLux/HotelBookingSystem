using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsManager
{
    public class WeekendPriceStrategy : IPriceStrategy
    {
        private const decimal WeekendMultiplier = 1.2m;
        private const decimal DefaultMultiplier = 1.0m;

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public decimal CalculatePrice(decimal basePrice, int days, DateTime checkInDate)
        {
            if (days < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(days), days, "Количество дней должно быть больше нуля");
            }

            decimal totalPrice = 0m;
            for (int i = 0; i < days; ++i)
            {
                DateTime date = checkInDate.AddDays(i);
                if (IsWeekend(date))
                {
                    totalPrice += basePrice * WeekendMultiplier;
                }
                else
                {
                    totalPrice += basePrice * DefaultMultiplier;
                }
            }

            return totalPrice;
        }
    }
}