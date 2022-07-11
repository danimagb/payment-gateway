namespace PaymentGateway.Application.Payments.Queries.GetById
{
    using MediatR;

    public class GetPaymentByIdQuery : IRequest<PaymentDetailsResponseDTO>
    {
        public Guid MerchantId { get; }
        public Guid PaymentId { get; }

        public GetPaymentByIdQuery(Guid merchantId, Guid paymentId)
        {
            this.MerchantId = merchantId;
            this.PaymentId = paymentId;
        }
    }
}
