namespace PaymentGateway.Domain.Exceptions
{
    public class InvalidCardNumberException : Exception
    {
        public InvalidCardNumberException()
           : base($"Card number is invalid")
        {
        }
    }
}
