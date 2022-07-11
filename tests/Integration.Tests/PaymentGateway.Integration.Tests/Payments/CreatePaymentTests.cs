namespace PaymentGateway.Integration.Tests.Payments
{
    using AutoFixture.Xunit2;

    using FluentAssertions;

    using Newtonsoft.Json;

    using PaymentGateway.Application.Payments;

    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    [Collection("DatabaseCollection")]
    public class CreatePaymentTests
    {
        private readonly TestServerFixture fixture;

        public CreatePaymentTests(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task CreatePaymentCommand_ValidPayment_ShouldReturnStatusCodeCreated()
        {
            // Arrange
            var createPaymentRequest = this.BuildCreatePaymentRequestDto(Guid.NewGuid(), "some holder");
            // Act
            var response = await this.fixture.paymentGatewayApiClient.PostAsync(
                "/payments",
                new StringContent(JsonConvert.SerializeObject(createPaymentRequest), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        private CreatePaymentRequestDTO BuildCreatePaymentRequestDto(Guid requestId, string holderName)
        {
            return new CreatePaymentRequestDTO
            {
                Amount = 10,
                CardDetails = new CardDetailsDTO
                {
                    Number = "4263 9826 4026 9299",
                    Cvv = "123",
                    ExpiryMonth = 12,
                    ExpiryYear = 2023,
                    Holder = holderName
                },
                Currency = "USD",
                RequestId = requestId,
            };
        }

    }
}
