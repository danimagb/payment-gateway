namespace PaymentGateway.Application.Payments.Queries.GetById
{
    using MediatR;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Domain.Services;

    using System.Threading;
    using System.Threading.Tasks;

    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDetailsResponseDTO>
    {
        private readonly ILogger<GetPaymentByIdQueryHandler> logger;
        private readonly IApplicationDbContext context;
        private readonly ICardNumberMaskingService maskingService;

        public GetPaymentByIdQueryHandler(ILogger<GetPaymentByIdQueryHandler> logger, IApplicationDbContext context, ICardNumberMaskingService maskingService)
        {
            this.logger = logger;
            this.context = context;
            this.maskingService = maskingService;
        }

        public async Task<PaymentDetailsResponseDTO> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment =  await this.context.Payments
                .FirstOrDefaultAsync(p => p.MerchantId.Equals(request.MerchantId) && p.Id.Equals(request.PaymentId), cancellationToken);
            
            if (payment == null)
            {
                return null;
            }

            // TODO: Extract to a mapper
            return new PaymentDetailsResponseDTO
            {
                Id = payment.Id,
                RequestId = payment.RequestId,
                CardDetails = new CardDetailsDTO
                {
                    Number = maskingService.Mask(payment.CardDetails.Number),
                    Cvv = payment.CardDetails.Cvv.Value,
                    ExpiryMonth = payment.CardDetails.ExpiryDate.Month,
                    ExpiryYear = payment.CardDetails.ExpiryDate.Year,
                    Holder = payment.CardDetails.HolderName
                },
                Amount = payment.Amount.Value,
                Currency = payment.Amount.Currency.Code,
                CreatedAt = payment.CreatedAt,
                ProcessedAt = payment.ProcessedAt,
                Status = payment.Status.ToString(),
                Message = payment.Message
            };
        }
    }
}
