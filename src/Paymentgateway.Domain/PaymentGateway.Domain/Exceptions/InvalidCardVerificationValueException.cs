namespace PaymentGateway.Domain.Exceptions
{
    public class InvalidCardVerificationValueException : Exception
    {
        public InvalidCardVerificationValueException()
            : base($"Invalid cvv")
        {
        }
    }
}
