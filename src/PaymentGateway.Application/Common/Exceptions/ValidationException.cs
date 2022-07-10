namespace PaymentGateway.Application.Common.Exceptions
{
    using PaymentGateway.Domain.Common;

    public class ValidationException : Exception
    {
        public ValidationException(string message, DomainValidationException ex)
            : base(message, ex)
        {
        }
    }
}
