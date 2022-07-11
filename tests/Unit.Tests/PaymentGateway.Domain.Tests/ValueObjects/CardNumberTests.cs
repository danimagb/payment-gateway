namespace PaymentGateway.Domain.Tests.ValueObjects
{
    using FluentAssertions;

    using PaymentGateway.Domain.Exceptions;
    using PaymentGateway.Domain.ValueObjects;

    using System.Diagnostics.CodeAnalysis;

    using Xunit;

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit.Domain.ValueObjects")]
    public class CardNumberTests
    {

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void Create_InvalidNumber_ShouldThrowException(string number)
        {
            // Arrange
            // Act
            var sut = () => CardNumber.Create(number);

            // Assert
            sut.Should().Throw<InvalidCardNumberException>();

        }

        [Theory]
        [InlineData("4263 9826 4026 9299")]
        [InlineData("4263  9826  4026  9299")]
        [InlineData("4263982640269299")]
        [InlineData("4263-9826-4026-9299")]
        public void Create_ValidNumber_ShouldCreateCardNumber(string number)
        {
            // Arrange
            // Act
            var cardNumber = CardNumber.Create(number);

            // Assert
            cardNumber.Should().NotBeNull();
            cardNumber.Value.Should().Be(number);

        }
    }
}
