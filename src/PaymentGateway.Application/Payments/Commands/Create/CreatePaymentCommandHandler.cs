namespace PaymentGateway.Application.Payments.Commands.Create
{
    using MediatR;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using PaymentGateway.Application.Common.Exceptions;
    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Domain.Common;
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.Entities;
    using PaymentGateway.Domain.ValueObjects;

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResponseDTO>
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

        public async Task<CreatePaymentResponseDTO> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existentPayment = await this.context.Payments.FirstOrDefaultAsync(p => p.MerchantId == request.MerchantId && p.RequestId == request.Payment.RequestId, cancellationToken);

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


                return new CreatePaymentResponseDTO
                {
                    Id = payment.Id,
                    RequestId = payment.RequestId,
                    CreatedAt = payment.CreatedAt,
                    ProcessedAt = payment.ProcessedAt,
                    Status = payment.Status.ToString(),
                    Message = payment.Message
                };
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

                if (paymentProcessedResponse.status is not PaymentStatus.Accepted)
                {
                    payment.Decline(paymentProcessedResponse.message);
                    return;
                }

                payment.Accept(paymentProcessedResponse.message);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the payment", new {paymentId = payment.Id});

                payment.Decline("Something went wrong when processing the payment. Contact the system administrator for more information");
                throw;
            }
        }
    }
}
