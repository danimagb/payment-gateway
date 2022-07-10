namespace PaymentsGateway.Infrastructure.AcquiringBank
{
    using PaymentGateway.Infrastructure.AcquiringBank;

    public class PaymentResponse
    {
        public Guid Id { get; set; }

        public PaymentRequestStatus Status { get; set; }   

        public string Message { get; set; }
    }
}
