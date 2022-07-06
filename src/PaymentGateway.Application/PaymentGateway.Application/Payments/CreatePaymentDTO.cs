namespace PaymentGateway.Application.Payments
{
    public class CreatePaymentDTO
    {
        public Guid RequestId { get; set; }
        
        public decimal Ammount { get; set; }

        public string Currency { get; set; }

        public CardDetailsDTO CardDetails { get; set; }
    }
}
