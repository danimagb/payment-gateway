namespace PaymentGateway.Domain.Exceptions
{
    public class InvalidCardExpiryDateException : Exception
    {
        public InvalidCardExpiryDateException(string datePart)
            : base($"Card expiry date is invalid. Invalid date part: {datePart}")
        {
        }
    }
}
