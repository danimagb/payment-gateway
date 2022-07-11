namespace PaymentGateway.Application.Payments
{
    public class PaymentDetailsResponseDTO
    {
        public Guid Id { get; set; }

        public Guid RequestId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public CardDetailsDTO CardDetails { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string Status { get; set; }

        public string Message { get; set; }

    }
}
