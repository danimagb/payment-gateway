namespace PaymentGateway.Domain.Tests.ValueObjects
{
    using FluentAssertions;

    using PaymentGateway.Domain.Exceptions;
    using PaymentGateway.Domain.ValueObjects;

    using System.Diagnostics.CodeAnalysis;

    using Xunit;

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit.Domain.ValueObjects")]
    public class CardVerificationValueTests
    {

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("1")]
        [InlineData("aaa")]
        [InlineData("a123")]
        [InlineData("aa")]
        [InlineData("a1")]
        [InlineData("1 2")]
        [InlineData("1 2 ")]
        [InlineData("a a")]
        [InlineData("a a ")]
        public void Create_InvalidCvv_ShouldThrowException(string value)
        {
            // Arrange
            // Act
            var sut = () => CardVerificationValue.Create(value);

            // Assert
            sut.Should().Throw<InvalidCardVerificationValueException>();

        }

        [Theory]
        [InlineData("111")]
        [InlineData("1111")]
        public void Create_ValidCvv_ShouldCreateCardNumber(string value)
        {
            // Arrange
            // Act
            var cardVerificationValue = CardVerificationValue.Create(value);

            // Assert
            cardVerificationValue.Should().NotBeNull();
            cardVerificationValue.Value.Should().Be(value);

        }
    }
}
