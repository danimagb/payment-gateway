namespace PaymentGateway.Domain.Tests.Services
{
    using AutoFixture.Xunit2;

    using FluentAssertions;

    using PaymentGateway.Domain.Services;
    using PaymentGateway.Domain.ValueObjects;

    using System.Diagnostics.CodeAnalysis;

    using Xunit;

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit.Domain.Services")]
    public class CardNumberMaskingServiceTests
    {

        [Theory]
        [InlineData("4263 9826 4026 9299", "4263 98******* 9299")]
        [InlineData("4263982640269299", "426398******9299")]
        [InlineData("4263-9826-4026-9299", "4263-98*******-9299")]
        public void Mask_ShouldApplyMasking(string number, string maskedNumber)
        {
            // Arrange
            var cardNumber = CardNumber.Create(number);

            var sut = new CardNumberMaskingService();

            // Act
            var maskedCardNumber = sut.Mask(cardNumber);

            // Assert
            maskedCardNumber.Should().Be(maskedNumber);
        }
    }
}
