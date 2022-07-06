namespace PaymentGateway.Domain.ValueObjects
{
    public class CardDetails
    {
        public string Number { get; }

        public string Cvv { get; }

        public CardExpiryDate ExpiryDate { get; }

        public string HolderName { get; }

        private CardDetails(string number, string cvv, CardExpiryDate expiryDate, string holderName)
        {
            this.Number = number;
            this.Cvv = cvv;
            this.ExpiryDate = expiryDate;
            this.HolderName = holderName;           
        }

        public static CardDetails Create(string number, string cvv, CardExpiryDate expiryDate, string holderName)
        {
            //TODO: Extra validations might be needed based on the card checksum digits of each card type (e.g: Visa, MasterCard)

            return new CardDetails(number, cvv, expiryDate, holderName);
        }

    }
}
