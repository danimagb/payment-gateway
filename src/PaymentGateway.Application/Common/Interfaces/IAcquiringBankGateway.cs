namespace PaymentGateway.Application.Common.Interfaces
{
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.Payments;

    public interface IAcquiringBankGateway
    {
        Task<PaymentStatus> ProcessPaymentAsync(Payment request);
    }
}
