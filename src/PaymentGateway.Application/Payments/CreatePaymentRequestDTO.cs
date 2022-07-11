namespace PaymentGateway.Application.Payments
{
    public class CreatePaymentRequestDTO
    {
        public Guid RequestId { get; set; }
        
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public CardDetailsDTO CardDetails { get; set; }
    }
}
