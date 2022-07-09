namespace PaymentGateway.Application.Payments.Commands.Create
{
    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.Exceptions;
    using PaymentGateway.Domain.Payments;
    using PaymentGateway.Domain.ValueObjects;

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Guid>
    {
        private readonly IApplicationDbContext context;
        private readonly IAcquiringBankGateway acquiringBankGateway;

        public CreatePaymentCommandHandler(IApplicationDbContext context, IAcquiringBankGateway acquiringBankGateway)
        {
            this.context = context;
            this.acquiringBankGateway = acquiringBankGateway;
        }

        public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {

            var existentPayment = await this.context.Payments.FirstOrDefaultAsync(p => p.MerchantId.Equals(request.MerchantId) && p.RequestId.Equals(request.Payment.RequestId), cancellationToken);

            if(existentPayment is not null)
            {
                throw new DuplicatedPaymentException(request.Payment.RequestId, existentPayment.Id);
            }

            var cardNumber = CardNumber.Create(request.Payment.CardDetails.Number);
            var cardVerificationValue = CardVerificationValue.Create(request.Payment.CardDetails.Cvv);
            var cardExpirityDate = CardExpiryDate.Create(request.Payment.CardDetails.ExpiryMonth, request.Payment.CardDetails.ExpiryYear);

            var cardDetails = CardDetails.Create(
                cardNumber,
                cardVerificationValue,
                cardExpirityDate,
                request.Payment.CardDetails.Holder);

            var paymentCurrency = PaymentCurrency.Create(request.Payment.Currency);
            var paymentAmount = new PaymentAmount(request.Payment.Amount, paymentCurrency);

            var payment = new Payment(
                request.Payment.RequestId,
                request.MerchantId,
                paymentAmount,
                cardDetails);

            await this.ProcessPaymentAsync(payment);


            await this.context.Payments.AddAsync(payment, cancellationToken);


            await this.context.SaveChangesAsync(cancellationToken);


            return payment.Id;
        }

        private async Task ProcessPaymentAsync(Payment payment)
        {
            try
            {
                var paymentProcessedResponse = await acquiringBankGateway.ProcessPaymentAsync(payment);

                if (paymentProcessedResponse is not PaymentStatus.Accepted)
                {
                    payment.Decline();
                    return;
                }

                payment.Accept();

            }
            catch (Exception)
            {
                payment.Decline();
                throw;
            }
        }
    }
}
