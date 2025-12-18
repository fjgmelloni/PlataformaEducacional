using Bogus;
using FluentAssertions;
using PlataformaEducacional.Core.Domain;

namespace PlataformaEducacional.Core.Tests
{
    public class GuardTests
    {
        private Faker Faker { get; } = new Faker("pt_BR");

        [Fact(DisplayName = nameof(AgainstNullOrWhiteSpace_ShouldNotThrow_WhenValueIsValid))]
        [Trait("Category", "Core - Guard")]
        public void AgainstNullOrWhiteSpace_ShouldNotThrow_WhenValueIsValid()
        {
            // Arrange
            var message = Faker.Commerce.ProductName().Replace(" ", "");
            var value = Faker.Commerce.ProductName();

            // Act
            Action action = () => Guard.AgainstNullOrWhiteSpace(value, message);

            // Assert
            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(AgainstNullOrWhiteSpace_ShouldThrowException_WhenValueIsNullOrEmpty))]
        [Trait("Category", "Core - Guard")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void AgainstNullOrWhiteSpace_ShouldThrowException_WhenValueIsNullOrEmpty(string? value)
        {
            // Arrange
            var message = Faker.Commerce.ProductName().Replace(" ", "");

            // Act
            Action action = () => Guard.AgainstNullOrWhiteSpace(value, message);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage(message);
        }

        [Fact(DisplayName = nameof(AgainstLessOrEqualDecimal_ShouldNotThrow_WhenValueIsGreater))]
        [Trait("Category", "Core - Guard")]
        public void AgainstLessOrEqualDecimal_ShouldNotThrow_WhenValueIsGreater()
        {
            // Arrange
            var message = Faker.Commerce.ProductName().Replace(" ", "");
            var value = Faker.Finance.Amount(min: 1.00m, max: 100.00m, decimals: 2);

            // Act
            Action action = () => Guard.AgainstLessOrEqual(value, 0, message);

            // Assert
            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(AgainstLessOrEqualDecimal_ShouldThrowException_WhenValueIsLessOrEqual))]
        [Trait("Category", "Core - Guard")]
        public void AgainstLessOrEqualDecimal_ShouldThrowException_WhenValueIsLessOrEqual()
        {
            // Arrange
            var message = Faker.Commerce.ProductName().Replace(" ", "");
            var value = 10m;

            // Act
            Action action = () => Guard.AgainstLessOrEqual(value, 20, message);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage(message);
        }

        [Fact(DisplayName = nameof(AgainstLessOrEqualInt_ShouldThrowException_WhenValueIsLessOrEqual))]
        [Trait("Category", "Core - Guard")]
        public void AgainstLessOrEqualInt_ShouldThrowException_WhenValueIsLessOrEqual()
        {
            // Arrange
            var message = Faker.Commerce.ProductName().Replace(" ", "");
            var value = 5;

            // Act
            Action action = () => Guard.AgainstLessOrEqual(value, 10, message);

            // Assert
            action.Should()
                .Throw<DomainException>()
                .WithMessage(message);
        }
    }
}
