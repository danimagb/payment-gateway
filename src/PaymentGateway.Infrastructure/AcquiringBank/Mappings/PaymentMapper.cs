namespace PaymentGateway.Infrastructure.AcquiringBank.Mappings
{
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.Entities;
    using PaymentsGateway.Infrastructure.AcquiringBank;

    /// <summary>
    /// Simple mapper just to demonstrate the ACL pattern when communicating with external api's
    /// </summary>
    public static class PaymentMapper
    {

        public static PaymentRequest MapToAcquiringBank(this Payment payment)
        {
            return payment is null
                ? null
                : new PaymentRequest
                {
                    CardHolder = payment.CardDetails.HolderName,
                    CardNumber = payment.CardDetails.Number.Value,
                    Cvv = int.Parse(payment.CardDetails.Cvv.Value),
                    Amount = payment.Amount.Value,
                    Currency = payment.Amount.Currency.Code,
                    ExpiryDate = $"{payment.CardDetails.ExpiryDate.Month}/{payment.CardDetails.ExpiryDate.Year}"
                };
        }

        public static (PaymentStatus status, string message) MapToDomain(this PaymentResponse paymentResponse)
        {
            if(paymentResponse.Status is not AcquiringBank.PaymentRequestStatus.Accepted)
            {
                return (PaymentStatus.Declined, paymentResponse.Message);
            }

            return (PaymentStatus.Accepted, paymentResponse.Message);
        }
    }
}
