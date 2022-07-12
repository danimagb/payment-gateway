namespace PaymentGateway.Domain.Exceptions
{
    using PaymentGateway.Domain.Common;

    public class InvalidCardExpiryDateException : DomainValidationException
    {
        public InvalidCardExpiryDateException(string datePart)
            : base($"Card expiry date is invalid. Invalid date part: {datePart}")
        {
        }
    }
}
