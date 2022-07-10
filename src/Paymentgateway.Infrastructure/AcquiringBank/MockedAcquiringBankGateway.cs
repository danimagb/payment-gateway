namespace PaymentGateway.Infrastructure.Gateways
{
    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.Payments;
    using PaymentGateway.Infrastructure.AcquiringBank;
    using PaymentGateway.Infrastructure.AcquiringBank.Mappings;

    using PaymentsGateway.Infrastructure.AcquiringBank;

    using System.Threading.Tasks;

    public class MockedAcquiringBankGateway : IAcquiringBankGateway
    {
        public async Task<PaymentStatus> ProcessPaymentAsync(Payment payment)
        {
            var paymentRequest = PaymentMapper.MapToAcquiringBank(payment);

            var response = await SendRequestAsync(paymentRequest);

            return response.MapToDomain();
        }

        /// <summary>
        /// Simulates the Http call to the acquiring bank
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        private async ValueTask<PaymentResponse> SendRequestAsync(PaymentRequest paymentRequest)
        {
            await Task.Delay(300);

            return await ValueTask.FromResult(new PaymentResponse
            {
                Id = Guid.NewGuid(),
                Status = PaymentRequestStatus.Accepted,
                Message = "Payment successfully executed"
            });
        }
    }
}
