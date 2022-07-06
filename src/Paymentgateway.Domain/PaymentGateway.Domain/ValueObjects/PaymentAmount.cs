namespace PaymentGateway.Domain.ValueObjects
{
    public class PaymentAmount
    {
        public decimal Ammount { get; }

        public Currency Currency { get; }

        private PaymentAmount(decimal ammount, Currency currency)
        {
            this.Ammount = ammount;
            this.Currency = currency;
        }

        public static PaymentAmount Create(decimal ammount, Currency currency)
        {
            return new PaymentAmount(ammount, currency);
        }
    }
}
