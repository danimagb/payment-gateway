namespace PaymentGateway.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;

    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.Entities;

    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Payment> Payments { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var paymentEntity = modelBuilder
            .Entity<Payment>()
            .ToTable("payments");

            paymentEntity.Property(p => p.Id).HasColumnName("id");
            paymentEntity.Property(p => p.RequestId).HasColumnName("request_id");
            paymentEntity.Property(p => p.MerchantId).HasColumnName("merchant_id");
            paymentEntity.Property(p => p.CreatedAt).HasColumnName("created_at");
            paymentEntity.Property(p => p.ProcessedAt).HasColumnName("processed_at");
            paymentEntity.Property(p => p.Status).HasColumnName("status").HasConversion( 
                s => s.ToString(), 
                s=>(PaymentStatus)Enum.Parse(typeof(PaymentStatus), s)
            );
            paymentEntity.Property(p => p.Message).HasColumnName("status_message");

            paymentEntity.OwnsOne(payment => payment.CardDetails,
                cardDetails =>
                {
                    cardDetails.Property(p => p.HolderName).HasColumnName("card_holder_name");

                    cardDetails.OwnsOne(cardDetails => cardDetails.Number,
                        number =>
                        {
                            number.Property(p => p.Value).HasColumnName("card_number");
                        });

                    cardDetails.OwnsOne(cardDetails => cardDetails.Cvv,
                        cvv =>
                        {
                            cvv.Property(p => p.Value).HasColumnName("card_cvv");
                        });

                    cardDetails.OwnsOne(cardDetails => cardDetails.ExpiryDate,
                        expiryDate =>
                        {
                            expiryDate.Property(p => p.Month).HasColumnName("card_expiry_month");
                            expiryDate.Property(p => p.Year).HasColumnName("card_expiry_year");
                        });
                });

            paymentEntity.OwnsOne(payment => payment.Amount,
                amount =>
                {
                    amount.Property(p => p.Value).HasColumnName("amount");
                    amount.OwnsOne(amount => amount.Currency,
                        currency =>
                        {
                            currency.Property(p => p.Code).HasColumnName("currency");
                        });
                });
        }
    }
}
