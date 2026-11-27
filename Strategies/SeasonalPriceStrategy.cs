using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsManager
{
    public class SeasonalPriceStrategy : IPriceStrategy
    {
        private const decimal SummerMultiplier = 1.3m;
        private const decimal HolidayMultiplier = 1.5m;
        private const decimal DefaultMultiplier = 1.0m;

        private const int HolidayStartMonth = 12;
        private const int HolidayStartDay = 20;
        private const int HolidayEndMonth = 1;
        private const int HolidayEndDay = 10;

        private const int FirstSummerMonth = 6;
        private const int LastSummerMonth = 8;

        private static bool IsHolidayDate(DateTime date)
        {
            return (date.Month == HolidayStartMonth && date.Day >= HolidayStartDay)
                   || (date.Month == HolidayEndMonth && date.Day <= HolidayEndDay);
        }

        private static bool IsSummerDate(DateTime date)
        {
            return date.Month >= FirstSummerMonth && date.Month <= LastSummerMonth;
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

                if (IsHolidayDate(date))
                {
                    totalPrice += basePrice * HolidayMultiplier;
                }
                else if (IsSummerDate(date))
                {
                    totalPrice += basePrice * SummerMultiplier;
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