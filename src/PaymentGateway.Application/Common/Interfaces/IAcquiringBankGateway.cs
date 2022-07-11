namespace PaymentGateway.Application.Common.Interfaces
{
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.Entities;

    public interface IAcquiringBankGateway
    {
        Task<(PaymentStatus status, string message)> ProcessPaymentAsync(Payment request);
    }
}
