namespace PaymentGateway.Application.Common.Exceptions
{
    public class DuplicatedPaymentException : Exception
    {
        public DuplicatedPaymentException(Guid requestId, Guid paymentId)
            : base($"A payment with RequestId {requestId} has already been created. Check the Payment Details for the payment Id:{paymentId}")
        {
        }
    }
}
