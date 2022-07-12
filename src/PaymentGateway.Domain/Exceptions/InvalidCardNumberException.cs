namespace PaymentGateway.Domain.Exceptions
{
    using PaymentGateway.Domain.Common;

    public class InvalidCardNumberException : DomainValidationException
    {
        public InvalidCardNumberException()
           : base($"Card number is invalid")
        {
        }
    }
}
