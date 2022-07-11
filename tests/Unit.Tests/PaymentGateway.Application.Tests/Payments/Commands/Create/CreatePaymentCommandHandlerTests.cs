namespace PaymetGateway.Application.Tests.Payments.Commands.Create
{
    using AutoFixture.Xunit2;

    using FluentAssertions;

    using Microsoft.Extensions.Logging;

    using MockQueryable.Moq;

    using Moq;

    using PaymentGateway.Application.Common.Exceptions;
    using PaymentGateway.Application.Common.Interfaces;
    using PaymentGateway.Application.Payments.Commands.Create;
    using PaymentGateway.Domain.Entities;
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.ValueObjects;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    using DTO = PaymentGateway.Application.Payments;

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit.Application.CommandHandlers")]
    public class CreatePaymentCommandHandlerTests
    {
        private readonly Mock<ILogger<CreatePaymentCommandHandler>> mockLogger;
        private readonly Mock<IApplicationDbContext> mockContext;
        private readonly Mock<IAcquiringBankGateway> mockIAcquiringBankGateway;

        public CreatePaymentCommandHandlerTests()
        {
            mockLogger = new Mock<ILogger<CreatePaymentCommandHandler>>();
            mockContext = new Mock<IApplicationDbContext>();
            mockIAcquiringBankGateway = new Mock<IAcquiringBankGateway>();
        }

        [Theory]
        [AutoData]
        public async Task Handle_GivenValidPaymentCommand_ShouldReturnPaymentId(Guid requestId, Guid merchantId, string holderName)
        {
            // Arrange
            var createPaymentDto = this.BuildCreatePaymentDto(requestId, holderName);

            var mockPaymentSet = new List<Payment>().AsQueryable().BuildMockDbSet();
            mockContext.Setup(m => m.Payments).Returns(mockPaymentSet.Object);
            mockIAcquiringBankGateway.Setup(x => x.ProcessPaymentAsync(It.IsAny<Payment>())).ReturnsAsync((PaymentStatus.Accepted, It.IsAny<string>()));

            var command = new CreatePaymentCommand(merchantId, createPaymentDto);
            var sut = new CreatePaymentCommandHandler(mockLogger.Object, mockContext.Object, mockIAcquiringBankGateway.Object);

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            this.mockContext.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
            this.mockIAcquiringBankGateway.Verify(x => x.ProcessPaymentAsync(It.IsAny<Payment>()), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task Handle_GivenDuplicatedPayment_ShouldThrowException(Guid requestId, Guid merchantId, string holderName)
        {
            // Arrange
            var createPaymentDto = this.BuildCreatePaymentDto(requestId, holderName);

            var mockPaymentSet = new List<Payment>
            {
                this.BuildValidPayment(requestId, merchantId, holderName),
            }
            .AsQueryable()
            .BuildMockDbSet();

            mockContext.Setup(m => m.Payments).Returns(mockPaymentSet.Object);
            mockIAcquiringBankGateway.Setup(x => x.ProcessPaymentAsync(It.IsAny<Payment>())).ReturnsAsync((PaymentStatus.Accepted, It.IsAny<string>()));

            var command = new CreatePaymentCommand(merchantId, createPaymentDto);
            var sut = new CreatePaymentCommandHandler(mockLogger.Object, mockContext.Object, mockIAcquiringBankGateway.Object);

            // Act
            Func<Task> act = () => sut.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicatedPaymentException>();

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

        private DTO.CreatePaymentRequestDTO BuildCreatePaymentDto(Guid requestId, string holderName)
        {
            return new DTO.CreatePaymentRequestDTO
            {
                Amount = 10,
                CardDetails = new DTO.CardDetailsDTO
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
