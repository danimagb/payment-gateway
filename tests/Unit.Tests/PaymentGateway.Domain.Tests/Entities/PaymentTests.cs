namespace PaymentGateway.Domain.Tests.Entities
{
    using AutoFixture.Xunit2;

    using FluentAssertions;

    using PaymentGateway.Domain.Common;
    using PaymentGateway.Domain.Entities;
    using PaymentGateway.Domain.Enums;
    using PaymentGateway.Domain.ValueObjects;

    using System;
    using System.Diagnostics.CodeAnalysis;

    using Xunit;

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit.Domain.Entities")]
    public class PaymentTests
    {

        [Theory]
        [AutoData]
        public void Create_ValidaParameters_ShouldCreatePayment(Guid requestId, Guid merchantId, string holderName)
        {
            // Arrange
            var cardNumber = CardNumber.Create("4263982640269299");
            var cardVerificationValue = CardVerificationValue.Create("123");
            var cardExpirityDate = CardExpiryDate.Create(DateTime.UtcNow.Month, DateTime.UtcNow.Year);
            var cardDetails = CardDetails.Create(cardNumber,cardVerificationValue,cardExpirityDate,holderName);
            var paymentAmount = PaymentAmount.Create(10, PaymentCurrency.USD);


            // Act
            var payment = Payment.Create(requestId, merchantId, paymentAmount, cardDetails);

            // Assert
            payment.Should().NotBeNull();
            payment.RequestId.Should().Be(requestId);
            payment.MerchantId.Should().Be(merchantId);
            payment.CardDetails.Should().BeEquivalentTo(cardDetails);
            payment.Amount.Should().BeEquivalentTo(paymentAmount);
            payment.Status.Should().Be(PaymentStatus.Pending);
        }

        [Theory]
        [AutoData]
        public void Accept_ShouldChangePaymentStatusToAccepted(Guid requestId, Guid merchantId, string holderName, DateTime dateTime)
        {
            // Arrange
            var cardNumber = CardNumber.Create("4263982640269299");
            var cardVerificationValue = CardVerificationValue.Create("123");
            var cardExpirityDate = CardExpiryDate.Create(dateTime.Month, dateTime.Year);
            var cardDetails = CardDetails.Create(cardNumber, cardVerificationValue, cardExpirityDate, holderName);
            var paymentAmount = PaymentAmount.Create(10, PaymentCurrency.USD);
            var payment = Payment.Create(requestId, merchantId, paymentAmount, cardDetails);
            DateTimeProvider.Set(() => dateTime);
            var paymentMessage = "Success!";

            // Act
            payment.Accept(paymentMessage);

            // Assert
            payment.Status.Should().Be(PaymentStatus.Accepted);
            payment.ProcessedAt.Should().Be(dateTime);
            payment.Message.Should().Be(paymentMessage);
        }

        [Theory]
        [AutoData]
        public void Decline_ShouldChangePaymentStatusToDeclined(Guid requestId, Guid merchantId, string holderName, DateTime dateTime)
        {
            // Arrange
            var cardNumber = CardNumber.Create("4263982640269299");
            var cardVerificationValue = CardVerificationValue.Create("123");
            var cardExpirityDate = CardExpiryDate.Create(DateTime.UtcNow.Month, DateTime.UtcNow.Year);
            var cardDetails = CardDetails.Create(cardNumber, cardVerificationValue, cardExpirityDate, holderName);
            var paymentAmount = PaymentAmount.Create(10, PaymentCurrency.USD);
            var payment = Payment.Create(requestId, merchantId, paymentAmount, cardDetails);
            DateTimeProvider.Set(() => dateTime);
            var paymentMessage = "oh no, payment not processed :(";

            // Act
            payment.Decline(paymentMessage);

            // Assert
            payment.Status.Should().Be(PaymentStatus.Declined);
            payment.ProcessedAt.Should().Be(dateTime);
            payment.Message.Should().Be(paymentMessage);
        }
    }
}
