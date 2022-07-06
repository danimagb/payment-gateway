namespace PaymentGateway.Application.Payments
{
    public class PaymentDetailsDTO
    {
        public Guid Id { get; set; }

        public Guid RequestId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public CardDetailsDTO CardDetails { get; set; }

        public DateTime RequestedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Status { get; set; }  
    }
}
