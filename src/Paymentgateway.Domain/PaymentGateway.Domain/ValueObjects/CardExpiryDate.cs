

namespace PaymentGateway.Domain.ValueObjects
{
    using PaymentGateway.Domain.Exceptions;

    public class CardExpiryDate
    {
        public int Month { get; }

        public int Year { get; }

        private CardExpiryDate(int month, int year)
        {
            this.Month = month;
            this.Year = year;
        }

        public static CardExpiryDate Create(int month, int year)
        {
           var cardExpiryDate = DateOnly.FromDateTime(new DateTime(year).AddMonths(month));

           if (cardExpiryDate < DateOnly.FromDateTime(DateTime.UtcNow))
           {
                throw new InvalidCardExpiryDateException();
           }

           return new CardExpiryDate(month, year);
        }
    }
}
