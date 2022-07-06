namespace PaymentGateway.Domain.Exceptions
{
    public class InvalidCardExpiryDateException : Exception
    {
        public InvalidCardExpiryDateException()
            : base($"Card has expired")
        {
        }
    }
}
