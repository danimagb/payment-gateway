namespace PaymentGateway.Domain.Services
{
    using PaymentGateway.Domain.ValueObjects;

    using System.Text.RegularExpressions;

    public class CardNumberMaskingService : ICardNumberMaskingService
    {
        private const string maskSchema = @"(?<=\d{4}\d{2})\d{2}\d{4}(?=\d{4})|(?<=\d{4}( |-)\d{2})\d{2}\1\d{4}(?=\1\d{4})";

        public string Mask(CardNumber cardNumber)
        {
            var reg = new Regex(maskSchema);

            var valueNoSpaces = cardNumber.Value.Replace(" ", string.Empty).Replace("-", string.Empty);

            return reg.Replace(valueNoSpaces, new MatchEvaluator((m) => new String('*', m.Length)));
        }
    }
}
