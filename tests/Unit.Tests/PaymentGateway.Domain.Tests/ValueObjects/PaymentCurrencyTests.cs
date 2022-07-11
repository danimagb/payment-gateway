namespace PaymentGateway.Domain.Tests.ValueObjects
{
    using FluentAssertions;

    using PaymentGateway.Domain.Exceptions;
    using PaymentGateway.Domain.ValueObjects;

    using System.Diagnostics.CodeAnalysis;

    using Xunit;

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit.Domain.ValueObjects")]
    public class PaymentCurrencyTests
    {

        [Theory]
        [InlineData("CAD")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_InvalidCurrencyCode_ShouldThrowException(string code)
        {
            // Arrange
            // Act
            var sut = () => PaymentCurrency.Create(code);

            // Assert
            sut.Should().Throw<UnsupportedCurrencyException>();

        }

        [Theory]
        [InlineData("EUR")]
        [InlineData("USD")]
        [InlineData("eur")]
        [InlineData("usd")]
        public void Create_ValidCurrencyCode_ShouldCreateCardNumber(string code)
        {
            // Arrange
            // Act
            var currency = PaymentCurrency.Create(code);

            // Assert
            currency.Code.Should().Be(code);

        }
    }
}
