namespace PaymentGateway.Application.Common.Interfaces
{
    using Microsoft.EntityFrameworkCore;

    using PaymentGateway.Domain.Entities;

    public interface IApplicationDbContext
    {
        public DbSet<Payment> Payments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
