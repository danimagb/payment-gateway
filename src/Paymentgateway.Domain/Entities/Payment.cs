namespace PaymentGateway.Domain.Entities
{
    using PaymentGateway.Domain.Common;
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.ValueObjects;

    public class Payment
    {
        public Guid Id { get; }

        public Guid RequestId { get; } = Guid.NewGuid();

        public Guid MerchantId { get; }

        public PaymentAmount Amount { get; }

        public PaymentStatus Status { get; private set; }

        public string Message { get; private set; }

        public CardDetails CardDetails { get; private set; }

        public DateTime CreatedAt { get; }

        public DateTime? ProcessedAt { get; private set; }

        private bool IsProcessed => this.Status is not PaymentStatus.Pending;

        private Payment(Guid requestId, Guid merchantId, PaymentAmount amount, CardDetails cardDetails)
            : this (Guid.NewGuid(), requestId, merchantId, PaymentStatus.Pending, string.Empty, DateTime.UtcNow)
        {
            this.Status = PaymentStatus.Pending;
            this.CreatedAt = DateTimeProvider.Now;
            this.RequestId = requestId;
            this.MerchantId = merchantId;
            this.Amount = amount;
            this.CardDetails = cardDetails;  
        }

        private Payment(Guid Id, Guid requestId, Guid merchantId, PaymentStatus status, string message, DateTime createdAt, DateTime? ProcessedAt = null)
        {
            this.Id = Id;
            this.RequestId = requestId;
            this.MerchantId = merchantId;
            this.Status = status;
            this.Message = message;
            this.CreatedAt = createdAt;
            this.ProcessedAt = ProcessedAt;
        }

        public void Accept(string message)
        {
            if (IsProcessed)
            {
                throw new InvalidOperationException();
            }

            this.Status = PaymentStatus.Accepted;
            this.ProcessedAt = DateTimeProvider.Now;
            this.Message = message;
        }

        public void Decline(string message)
        {
            if (IsProcessed)
            {
                throw new InvalidOperationException();
            }

            this.Status = PaymentStatus.Declined;
            this.ProcessedAt = DateTimeProvider.Now;
            this.Message = message;
        }

        public static Payment Create(Guid requestId, Guid merchantId, PaymentAmount amount, CardDetails cardDetails)
        {
            requestId = requestId.Equals(Guid.Empty) ? Guid.NewGuid() : requestId;

            return new Payment(requestId, merchantId, amount, cardDetails);
        }
    }
}
