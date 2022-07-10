namespace PaymentGateway.Application.Payments.Commands.Create
{
    using MediatR;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using PaymentGateway.Application.Common.Exceptions;
    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Domain.Common;
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.Payments;
    using PaymentGateway.Domain.ValueObjects;

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Guid>
    {
        private readonly ILogger<CreatePaymentCommandHandler> logger;
        private readonly IApplicationDbContext context;
        private readonly IAcquiringBankGateway acquiringBankGateway;

        public CreatePaymentCommandHandler(ILogger<CreatePaymentCommandHandler> logger, IApplicationDbContext context, IAcquiringBankGateway acquiringBankGateway)
        {
            this.logger = logger;
            this.context = context;
            this.acquiringBankGateway = acquiringBankGateway;
        }

        public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existentPayment = await this.context.Payments.FirstOrDefaultAsync(p => p.MerchantId.Equals(request.MerchantId) && p.RequestId.Equals(request.Payment.RequestId), cancellationToken);

                if (existentPayment is not null)
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
                var paymentAmount = PaymentAmount.Create(request.Payment.Amount, paymentCurrency);

                var payment = Payment.Create(
                    request.Payment.RequestId,
                    request.MerchantId,
                    paymentAmount,
                    cardDetails);

                await this.ProcessPaymentAsync(payment);


                await this.context.Payments.AddAsync(payment, cancellationToken);


                await this.context.SaveChangesAsync(cancellationToken);


                return payment.Id;
            }
            catch (DomainValidationException ex)
            {
                logger.LogDebug(ex, "A domain validation error occurred");
                throw new ValidationException(ex.Message, ex);
            }
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
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the payment");
                payment.Decline();
                throw;
            }
        }
    }
}
