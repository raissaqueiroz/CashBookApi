using CashBook.Core.Enums;
using CashBook.Domain.Entities;
using CashBook.Domain.Validators;
using FluentAssertions;
using Xunit;

namespace CashBook.UnitTests.Domain.Validators
{
    public class UserValidatorTests
    {
        private readonly UserValidator _validator = new();

        [Fact(DisplayName = "Given valid user When validated Then should be valid")]
        public void GivenValidUser_WhenValidated_ThenShouldBeValid()
        {
            // Arrange
            var user = new User("Raissa", "raissa@example.com", "123456");

            // Act
            var result = _validator.Validate(user);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact(DisplayName = "Given invalid email When validated Then should have error")]
        public void GivenInvalidEmail_WhenValidated_ThenShouldHaveError()
        {
            // Arrange
            var user = new User("Raissa", "invalid", "123456");

            // Act
            var result = _validator.Validate(user);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "email is invalid");
        }

        [Fact(DisplayName = "Given empty name When validated Then should have error")]
        public void GivenEmptyName_WhenValidated_ThenShouldHaveError()
        {
            // Arrange
            var user = new User("", "raissa@example.com", "123456");

            // Act
            var result = _validator.Validate(user);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "name is required");
        }

        [Fact(DisplayName = "Given short password When validated Then should have error")]
        public void GivenShortPassword_WhenValidated_ThenShouldHaveError()
        {
            // Arrange
            var user = new User("Raissa", "raissa@example.com", "123");

            // Act
            var result = _validator.Validate(user);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "min. 6 characters");
        }
    }
}
