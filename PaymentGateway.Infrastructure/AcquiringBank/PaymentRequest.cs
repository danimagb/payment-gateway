namespace PaymentsGateway.Infrastructure.AcquiringBank
{
    public class PaymentRequest
    {
        public string CardHolder { get; set; }

        public string CardNumber { get; set; }

        public int Cvv { get; set; }

        public string? ExpiryDate { get; set; }

        public string Currency { get; set; }

        public string Amount { get; set; }

    }
}
