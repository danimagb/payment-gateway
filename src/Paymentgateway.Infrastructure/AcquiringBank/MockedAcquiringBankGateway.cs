namespace PaymentGateway.Infrastructure.Gateways
{
    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.Entities;
    using PaymentGateway.Infrastructure.AcquiringBank;
    using PaymentGateway.Infrastructure.AcquiringBank.Mappings;

    using PaymentsGateway.Infrastructure.AcquiringBank;

    using System.Threading.Tasks;

    public class MockedAcquiringBankGateway : IAcquiringBankGateway
    {
        private const decimal MaxPaymentAmount = 100000; 

        public async Task<(PaymentStatus status, string message)> ProcessPaymentAsync(Payment payment)
        {
            var paymentRequest = PaymentMapper.MapToAcquiringBank(payment);

            var response = await SendRequestAsync(paymentRequest);

            return response.MapToDomain();
        }

        /// <summary>
        /// Simulates the Http call to the acquiring bank
        /// Mocked Acquiring bank rejects payments with amount > 100000
        /// In order to be able to have Accepted and Declined Payment Processing flows
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        private async ValueTask<PaymentResponse> SendRequestAsync(PaymentRequest paymentRequest)
        {
            await Task.Delay(100);

            if(paymentRequest.Amount > MaxPaymentAmount)
            {
                return await ValueTask.FromResult(new PaymentResponse
                {
                    Id = Guid.NewGuid(),
                    Status = PaymentRequestStatus.Declined,
                    Message = "The payment amount is too high"
                });
            }

            return await ValueTask.FromResult(new PaymentResponse
            {
                Id = Guid.NewGuid(),
                Status = PaymentRequestStatus.Accepted,
                Message = "Payment successfully executed"
            });
        }
    }
}
