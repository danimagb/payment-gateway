namespace PaymentGateway.Domain.Payments
{
    using PaymentGateway.Domain.Common;
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.ValueObjects;

    public class Payment : BaseEntity
    {
        

        public Guid RequestId { get; }

        public Guid MerchantId { get; }

        public PaymentAmount Amount { get; }

        public PaymentStatus Status { get; private set; }

        public CardDetails CardDetails { get; private set; }

        public DateTime RequestedAt { get; }

        public DateTime UpdatedAt { get; private set; }


        private Payment(Guid requestId, Guid merchantId, PaymentAmount amount, CardDetails cardDetails)
            : base(Guid.NewGuid())
        {
            this.Status = PaymentStatus.Pending;
            this.RequestedAt = DateTime.UtcNow;
            this.RequestId = requestId;
            this.MerchantId = merchantId;
            this.Amount = amount;
            this.CardDetails = cardDetails; 
        }

        public void Accept()
        {
            this.Status = PaymentStatus.Accepted;
            this.UpdatedAt = DateTime.UtcNow;
        }

        public void Decline()
        {
            this.Status = PaymentStatus.Declined;
            this.UpdatedAt = DateTime.UtcNow;
        }

        public static Payment Create(Guid requestId, Guid merchantId, PaymentAmount amount, CardDetails cardDetails)
        {
            return new Payment(requestId, merchantId, amount, cardDetails);
        }

    }
}
