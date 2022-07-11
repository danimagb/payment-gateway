namespace PaymentGateway.Application.Payments.Commands.Create
{
    using MediatR;

    public class CreatePaymentCommand : IRequest<CreatePaymentResponseDTO>
    {
        public Guid MerchantId { get; }

        public CreatePaymentRequestDTO Payment { get; }

        public CreatePaymentCommand(Guid merchantId, CreatePaymentRequestDTO payment)
        {
            this.MerchantId = merchantId;
            this.Payment = payment;
        }
    }
}
