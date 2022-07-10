namespace PaymentGateway.Domain.ValueObjects
{
    using PaymentGateway.Domain.Exceptions;

    public class PaymentCurrency 
    {
        public static PaymentCurrency USD => new("USD");

        public static PaymentCurrency EUR => new("EUR");

        public string Code { get; private set; }


        private PaymentCurrency(string code)
        {
            this.Code = code;
        }

        protected static IEnumerable<PaymentCurrency> SupportedCurrencies
        {
            get
            {
                yield return USD;
                yield return EUR;
            }
        }

        public static PaymentCurrency Create(string code)
        {
            if (SupportedCurrencies.FirstOrDefault(currency => currency.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase)) is null)
            {
                throw new UnsupportedCurrencyException(code);
            }

            return new PaymentCurrency(code);
        }
    }
}
