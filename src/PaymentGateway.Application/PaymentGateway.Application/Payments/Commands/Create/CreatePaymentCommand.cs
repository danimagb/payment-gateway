namespace PaymentGateway.Application.Payments.Commands.Create
{
    using MediatR;

    public class CreatePaymentCommand : IRequest<Guid>
    {
        public Guid MerchantId { get; }

        public CreatePaymentDTO Payment { get; }

        public CreatePaymentCommand(Guid merchantId, CreatePaymentDTO payment)
        {
            this.MerchantId = merchantId;
            this.Payment = payment;
        }
    }
}
