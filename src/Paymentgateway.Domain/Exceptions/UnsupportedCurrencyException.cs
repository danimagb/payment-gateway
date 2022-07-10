namespace PaymentGateway.Domain.Exceptions
{
    using PaymentGateway.Domain.Common;

    public class UnsupportedCurrencyException : DomainValidationException
    {
        public UnsupportedCurrencyException(string code)
            : base($"Currency \"{code}\" is unsupported")
        {
        }
    }
}
