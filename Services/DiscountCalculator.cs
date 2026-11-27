namespace HotelsManager
{
    public static class DiscountCalculator
    {
        private const int LongStayThreshold = 7;
        private const int VeryLongStayThreshold = 14;

        private const decimal NoDiscount = 0m;
        private const decimal LongStayDiscount = 0.10m;
        private const decimal VeryLongStayDiscount = 0.15m;

        public static decimal GetDiscountRate(int days)
        {
            if (days < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(days), days, "Количество ночей должно быть больше нуля.");
            }

            if (days >= VeryLongStayThreshold)
            {
                return VeryLongStayDiscount;
            }

            if (days >= LongStayThreshold)
            {
                return LongStayDiscount;
            }

            return NoDiscount;
        }

        public static decimal ApplyDiscount(decimal price, int days)
        {
            decimal rate = GetDiscountRate(days);
            return price * (1 - rate);
        }
    }
}