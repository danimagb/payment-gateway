namespace PaymetGateway.Application.Tests.Payments.Commands.Create
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Application.Payments.Queries.GetById;
    using PaymentGateway.Domain.Entities;
    using PaymentGateway.Domain.ValueObjects;

    using AutoFixture.Xunit2;
    using Xunit;

    using FluentAssertions;

    using Microsoft.Extensions.Logging;

    using MockQueryable.Moq;

    using Moq;

    using DTO = PaymentGateway.Application.Payments;
    using PaymentGateway.Domain.Services;

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit.Application.QueryHandlers")]
    public class GetPaymentByIdQueryHandlerTests
    {
        private readonly Mock<ILogger<GetPaymentByIdQueryHandler>> mockLogger;
        private readonly Mock<IApplicationDbContext> mockContext;
        private readonly Mock<ICardNumberMaskingService> mockCardNumberMaskingService;

        public GetPaymentByIdQueryHandlerTests()
        {
            mockLogger = new Mock<ILogger<GetPaymentByIdQueryHandler>>();
            mockContext = new Mock<IApplicationDbContext>();
            mockCardNumberMaskingService = new Mock<ICardNumberMaskingService>();
        }

        [Theory]
        [AutoData]
        public async Task Handle_GivenValidPaymentCommand_ShouldReturnPaymentId(Guid requestId, Guid merchantId, string holderName)
        {
            // Arrange
            var existentPayment = this.BuildValidPayment(requestId, merchantId, holderName);
            var mockPaymentSet = new List<Payment>
            {
                existentPayment
            }
            .AsQueryable()
            .BuildMockDbSet();

            mockContext.Setup(m => m.Payments).Returns(mockPaymentSet.Object);
            mockCardNumberMaskingService.Setup(x => x.Mask(It.IsAny<CardNumber>())).Returns(existentPayment.CardDetails.Number.Value);

            var expected = BuildPaymentDetailsDtoFromPayment(existentPayment);
            var command = new GetPaymentByIdQuery(merchantId, existentPayment.Id);
            var sut = new GetPaymentByIdQueryHandler(mockLogger.Object, mockContext.Object, mockCardNumberMaskingService.Object);

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        private Payment BuildValidPayment(Guid requestId, Guid merchantId, string holderName)
        {
            var cardNumber = CardNumber.Create("4263982640269299");
            var cardVerificationValue = CardVerificationValue.Create("123");
            var cardExpirityDate = CardExpiryDate.Create(DateTime.UtcNow.Month, DateTime.UtcNow.Year);
            var cardDetails = CardDetails.Create(cardNumber, cardVerificationValue, cardExpirityDate, holderName);
            var paymentAmount = PaymentAmount.Create(10, PaymentCurrency.USD);

            return Payment.Create(requestId, merchantId, paymentAmount, cardDetails);
        }

        private DTO.PaymentDetailsResponseDTO BuildPaymentDetailsDtoFromPayment(Payment payment)
        {
            return new DTO.PaymentDetailsResponseDTO
            {
                Id = payment.Id,
                Amount = payment.Amount.Value,
                CardDetails = new DTO.CardDetailsDTO
                {
                    Number = payment.CardDetails.Number.Value,
                    Cvv = payment.CardDetails.Cvv.Value,
                    ExpiryMonth = payment.CardDetails.ExpiryDate.Month,
                    ExpiryYear = payment.CardDetails.ExpiryDate.Year,
                    Holder = payment.CardDetails.HolderName
                },
                Currency = payment.Amount.Currency.Code,
                RequestId = payment.RequestId,
                CreatedAt = payment.CreatedAt,
                ProcessedAt = payment.ProcessedAt,
                Status = payment.Status.ToString(),
            };
        }
    }
}
