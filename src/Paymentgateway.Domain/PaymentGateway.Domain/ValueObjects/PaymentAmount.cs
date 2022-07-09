namespace PaymentGateway.Domain.ValueObjects
{
    public class PaymentAmount
    {
        private readonly decimal MininumPaymentValue = 0m;

        public decimal Value { get; private set; }

        public PaymentCurrency Currency { get; private set; }

        public PaymentAmount(decimal value, PaymentCurrency currency)
            : this(value)
        {
            this.Currency = currency;
        }

        private PaymentAmount(decimal value)
        {
            this.Value = value;
        }
    }
}
