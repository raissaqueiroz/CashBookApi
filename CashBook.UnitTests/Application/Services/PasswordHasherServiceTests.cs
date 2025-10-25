using CashBook.Core.Services;
using CashBook.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace CashBook.UnitTests.Application.Services
{
    public class PasswordHasherServiceTests
    {
        [Fact(DisplayName = "Given valid password When hashing Then should return hashed string")]
        public void GivenValidPassword_WhenHashing_ThenShouldReturnHashed()
        {
            // Arrange
            var service = new PasswordHasherService();
            var user = new User("Raissa", "raissa@example.com", "123456");

            // Act
            var hashed = service.HashPassword(user, user.Password);

            // Assert
            hashed.Should().NotBeNullOrWhiteSpace();
            hashed.Should().NotBe(user.Password);
        }

        [Fact(DisplayName = "Given hashed password When verifying correct password Then should succeed")]
        public void GivenHashedPassword_WhenVerifyingCorrectPassword_ThenShouldSucceed()
        {
            // Arrange
            var service = new PasswordHasherService();
            var user = new User("Raissa", "raissa@example.com", "123456");
            var hashed = service.HashPassword(user, user.Password);

            // Act
            var result = service.VerifyPassword(user, hashed, "123456");

            // Assert
            result.Should().Be(PasswordVerificationResult.Success);
        }

        [Fact(DisplayName = "Given hashed password When verifying wrong password Then should fail")]
        public void GivenHashedPassword_WhenVerifyingWrongPassword_ThenShouldFail()
        {
            // Arrange
            var service = new PasswordHasherService();
            var user = new User("Raissa", "raissa@example.com", "123456");
            var hashed = service.HashPassword(user, user.Password);

            // Act
            var result = service.VerifyPassword(user, hashed, "wrongpassword");

            // Assert
            result.Should().Be(PasswordVerificationResult.Failed);
        }
    }
}
