namespace PaymentGateway.Domain.ValueObjects
{
    using PaymentGateway.Domain.Exceptions;

    using System.Text.RegularExpressions;

    public class CardNumber
    {
        private static string cardNumberSchema = @"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$";
        public string Value { get; }

        private CardNumber(string value)
        {
            this.Value = value;
        }

        public static CardNumber Create(string value)
        {
            if (value is null)
            {
                throw new InvalidCardNumberException();
            }

            var valueWithNoSpaces = value.Replace(" ", string.Empty).Replace("-", string.Empty);

            if (!Regex.IsMatch(valueWithNoSpaces, cardNumberSchema))
            {
                throw new InvalidCardNumberException();
            }

            return new CardNumber(value);
        }
    }
}
