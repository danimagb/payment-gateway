namespace PaymentGateway.Domain.ValueObjects
{
    using PaymentGateway.Domain.Exceptions;
    public class Currency
    {
        public static Currency USD => new("USD");

        public static Currency EUR => new("EUR");

        public string Code { get; private set; } = "USD";


        private Currency(string code)
        {
            this.Code = code;
        }

        public static Currency Create(string code)
        {
            var currency = new Currency(code);

            if (SupportedCurrencies.FirstOrDefault(currency => currency.Code == code) is null)
            {
                throw new UnsupportedCurrencyException(code);
            }

            return currency;
        }

        protected static IEnumerable<Currency> SupportedCurrencies
        {
            get
            {
                yield return USD;
                yield return EUR;
            }
        }
    }
}
