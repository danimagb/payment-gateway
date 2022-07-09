namespace PaymentGateway.Application.Payments
{
    public class CardDetailsDTO
    {
        public string Holder { get; set; }

        public string Number { get; set; }

        public string Cvv { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }
    }
}
