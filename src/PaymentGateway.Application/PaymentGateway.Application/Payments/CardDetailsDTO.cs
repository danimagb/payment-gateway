namespace PaymentGateway.Application.Payments
{
    public class CardDetailsDTO
    {
        public string CardHolder { get; set; }

        public string CardNumber { get; set; }

        public int Cvv { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }
    }
}
