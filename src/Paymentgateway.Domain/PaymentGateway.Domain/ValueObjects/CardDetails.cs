namespace PaymentGateway.Domain.ValueObjects
{
    using System.Text.RegularExpressions;
    public class CardDetails
    {
        public CardNumber Number { get; }

        public CardVerificationValue Cvv { get; }

        public CardExpiryDate ExpiryDate { get; }

        public string HolderName { get; }

        private CardDetails(CardNumber cardNumber, CardVerificationValue cvv, CardExpiryDate expiryDate, string holderName) 
            : this (holderName)
        {
            this.Number = cardNumber;
            this.Cvv = cvv;
            this.ExpiryDate = expiryDate;        
        }

        private CardDetails (string holderName)
        {
            this.HolderName = holderName;
        }

        public static CardDetails Create(CardNumber cardNumber, CardVerificationValue cvv, CardExpiryDate expiryDate, string holderName)
        {
            return new CardDetails(cardNumber, cvv, expiryDate, holderName);
        }
    }
}
