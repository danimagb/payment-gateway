namespace PaymentGateway.Domain.Repositories
{
    using PaymentGateway.Domain.Payments;

    public interface IPaymentRepository
    {
        Task<Payment> GetAsync(Guid id);

        Task AddAsync(Payment payment);
    }
}
