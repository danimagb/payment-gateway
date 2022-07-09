namespace PaymentGateway.Application.Payments.Queries.GetById
{
    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Domain.Services;

    using System.Threading;
    using System.Threading.Tasks;

    internal class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDetailsDTO>
    {
        private readonly IApplicationDbContext context;
        private readonly ICardNumberMaskingService maskingService;

        public GetPaymentByIdQueryHandler(IApplicationDbContext context, ICardNumberMaskingService maskingService)
        {
            this.context = context;
            this.maskingService = maskingService;
        }

        public async Task<PaymentDetailsDTO> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            // TODO: Filter by merchant ID
            var payment =  await this.context.Payments.FirstOrDefaultAsync(p => p.Id.Equals(request.PaymentId), cancellationToken);
            
            if (payment == null)
            {
                return null;
            }

            // TODO: Extract to a mapper
            return new PaymentDetailsDTO
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
                ProcessedAt = payment.ProcessedAt.Value,
                Status = payment.Status.ToString(),
            };
        }
    }
}
