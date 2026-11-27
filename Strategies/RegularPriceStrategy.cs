using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsManager
{
    public class RegularPriceStrategy : IPriceStrategy
    {
        public decimal CalculatePrice(decimal basePrice, int days, DateTime checkInDate)
        {
            if (days > 0)
            {
                return basePrice * days;
            }

            throw new ArgumentOutOfRangeException(nameof(days), days, "Количество дней должно быть больше нуля");
        }
    }
}
