namespace PaymentGateway.Application.Payments
{
    public class CreatePaymentResponseDTO
    {
        public Guid Id { get; set; }

        public Guid RequestId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string Status { get; set; }

        public string Message { get; set; }

    }
}
