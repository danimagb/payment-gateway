namespace PaymentGateway.Api
{
    public class CreatePaymentRequest
    {
        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public int Cvv { get; set; }

        public string Currency { get; set; }

        public decimal Ammount { get; set; }
        
    }
}