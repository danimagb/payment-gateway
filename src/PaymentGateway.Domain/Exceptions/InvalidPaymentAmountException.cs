namespace PaymentGateway.Domain.Exceptions
{
    using PaymentGateway.Domain.Common;

    public class InvalidPaymentAmountException : DomainValidationException
    {
        public InvalidPaymentAmountException(decimal amount)
            : base($"Invalid amount: {amount}")
        {
        }
    }
}
