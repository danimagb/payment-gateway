namespace PaymentGateway.Domain.ValueObjects
{
    using PaymentGateway.Domain.Exceptions;

    using static System.Linq.Enumerable;

    public class CardExpiryDate
    {
        private static readonly int MinimumMonth = 1;
        private static readonly int MaximumMonth = 12;

        private static readonly int MinimumYearLength = 2;
        private static readonly int MaximumYearLength = 4;
        
        public int Month { get; }

        public int Year { get; }

        private CardExpiryDate(int month, int year)
        {

            this.Month = month;
            this.Year = year;
        } 

        public static CardExpiryDate Create(int month, int year)
        {
            if(!Range(MinimumMonth, MaximumMonth).Contains(month))
            {
                throw new InvalidCardExpiryDateException(nameof(Month));
            }

            if (year < 0 || !Range(MinimumYearLength, MaximumYearLength).Contains(year.ToString().Length))
            {
                throw new InvalidCardExpiryDateException(nameof(Year));
            }

            return new CardExpiryDate(month, year);
        }
    }
}
