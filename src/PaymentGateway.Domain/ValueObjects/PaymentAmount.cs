namespace PaymentGateway.Domain.ValueObjects
{
    using PaymentGateway.Domain.Exceptions;

    public class PaymentAmount
    {
        private static readonly decimal MininumPaymentValue = 0m;

        public decimal Value { get; private set; }

        public PaymentCurrency Currency { get; private set; }

        private PaymentAmount(decimal value, PaymentCurrency currency)
            : this(value)
        {
            this.Currency = currency;
        }

        private PaymentAmount(decimal value)
        {

            this.Value = value;
        }

        public static PaymentAmount Create(decimal value, PaymentCurrency currency)
        {
            if(value <= MininumPaymentValue)
            {
                throw new InvalidPaymentAmountException(value);
            }

            return new PaymentAmount(value, currency);  
        }
    }
}
