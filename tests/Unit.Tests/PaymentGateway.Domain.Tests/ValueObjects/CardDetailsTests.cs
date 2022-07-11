namespace PaymentGateway.Domain.Tests.ValueObjects
{
    using FluentAssertions;

    using PaymentGateway.Domain.ValueObjects;

    using System;
    using System.Diagnostics.CodeAnalysis;

    using Xunit;

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit.Domain.ValueObjects")]
    public class CardDetailsTests
    {

        [Fact]
        public void Create_ValidaParameters_ShouldCreateCardDetails()
        {
            // Arrange
            var cardNumber = CardNumber.Create("4263982640269299");
            var cardVerificationValue = CardVerificationValue.Create("123");
            var cardExpirityDate = CardExpiryDate.Create(DateTime.UtcNow.Month, DateTime.UtcNow.Year);
            var holderName = "some holder";


            // Act
            var cardDetails = CardDetails.Create(
                cardNumber,
                cardVerificationValue,
                cardExpirityDate,
                holderName);

            // Assert
            cardDetails.Should().NotBeNull();
            cardDetails.Number.Should().BeEquivalentTo(cardNumber);
            cardDetails.Cvv.Should().BeEquivalentTo(cardVerificationValue);
            cardDetails.ExpiryDate.Should().BeEquivalentTo(cardExpirityDate);
            cardDetails.HolderName.Should().Be(holderName);
        }
    }
}
