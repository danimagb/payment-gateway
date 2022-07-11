namespace PaymentGateway.Domain.Tests.ValueObjects
{
    using FluentAssertions;

    using PaymentGateway.Domain.Exceptions;
    using PaymentGateway.Domain.ValueObjects;

    using System;
    using System.Diagnostics.CodeAnalysis;

    using Xunit;

    [ExcludeFromCodeCoverage]
    [Trait("Category", "Unit.Domain.ValueObjects")]
    public class CardExpiryDateTests
    {

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(13)]
        public void Create_InvalidMonth_ShouldThrowException(int month)
        {
            // Arrange
            // Act
            var sut = () => CardExpiryDate.Create(month, DateTime.UtcNow.Year);

            // Assert
            sut.Should().Throw<InvalidCardExpiryDateException>();

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        public void Create_InvalidYear_ShouldThrowException(int year)
        {
            // Arrange
            // Act
            var sut = () => CardExpiryDate.Create(DateTime.UtcNow.Month, year);

            // Assert
            sut.Should().Throw<InvalidCardExpiryDateException>();

        }

        [Theory]
        [InlineData(1, 12)]
        [InlineData(2, 2021)]
        [InlineData(12, 234)]
        public void Create_ValidMonthAndYear_ShouldCreateCardExpiryDate(int month, int year)
        {
            // Arrange
            // Act
            var cardExpiryDate =  CardExpiryDate.Create(month, year);

            // Assert
            cardExpiryDate.Should().NotBeNull();
            cardExpiryDate.Month.Should().Be(month);
            cardExpiryDate.Year.Should().Be(year);

        }
    }
}
