namespace PaymentGateway.Domain.ValueObjects
{
    using PaymentGateway.Domain.Exceptions;

    using System.Text.RegularExpressions;

    public class CardVerificationValue
    {
        private static string cardVerificationValueSchema = @"^[0-9]{3,4}$";

        public string Value { get; }

        private CardVerificationValue(string value)
        {
            this.Value = value;
        }

        public static CardVerificationValue Create(string value)
        {
            value = Regex.Replace(value, @"\s", string.Empty);

            if (!Regex.IsMatch(value, cardVerificationValueSchema) || value is null)
            {
                throw new InvalidCardVerificationValueException();
            }

            return new CardVerificationValue(value);
        }  
    }
}
