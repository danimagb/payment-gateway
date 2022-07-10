namespace PaymentGateway.Domain.Exceptions
{
    using PaymentGateway.Domain.Common;

    public class InvalidCardVerificationValueException : DomainValidationException
    {
        public InvalidCardVerificationValueException()
            : base($"Invalid cvv")
        {
        }
    }
}
