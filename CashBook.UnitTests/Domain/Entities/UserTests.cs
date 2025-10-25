using System;
using CashBook.Core.Enums;
using CashBook.Core.Exceptions;
using CashBook.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CashBook.UnitTests.Domain.Entities
{
    public class UserTests
    {
        [Fact(DisplayName = "Given valid parameters When creating user Then should create successfully")]
        public void GivenValidParameters_WhenCreatingUser_ThenShouldCreateSuccessfully()
        {
            // Arrange
            var name = "Raissa";
            var email = "raissa@example.com";
            var password = "123456";

            // Act
            var user = new User(name, email, password);

            // Assert
            user.Should().NotBeNull();
            user.Name.Should().Be(name);
            user.Email.Should().Be(email);
            user.Password.Should().Be(password);
            user.Status.Should().Be(UserStatus.Unconfirmed);
            user.Id.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Given invalid email When validating Then should throw DomainException")]
        public void GivenInvalidEmail_WhenValidating_ThenShouldThrowDomainException()
        {
            // Arrange
            var user = new User("Raissa", "invalid-email", "123456");

            // Act
            var act = () => user.Validate();

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Invalid Fields*")
                .And.Errors.Should().Contain("email is invalid");
        }

        [Fact(DisplayName = "Given empty name When validating Then should throw DomainException")]
        public void GivenEmptyName_WhenValidating_ThenShouldThrowDomainException()
        {
            // Arrange
            var user = new User("", "raissa@example.com", "123456");

            // Act
            var act = () => user.Validate();

            // Assert
            act.Should().Throw<DomainException>()
                .And.Errors.Should().Contain("name is required");
        }

        [Fact(DisplayName = "Given short password When validating Then should throw DomainException")]
        public void GivenShortPassword_WhenValidating_ThenShouldThrowDomainException()
        {
            // Arrange
            var user = new User("Raissa", "raissa@example.com", "123");

            // Act
            var act = () => user.Validate();

            // Assert
            act.Should().Throw<DomainException>()
                .And.Errors.Should().Contain("min. 6 characters");
        }

        [Fact(DisplayName = "Given valid user When changing name Then should update successfully")]
        public void GivenValidUser_WhenChangingName_ThenShouldUpdateSuccessfully()
        {
            // Arrange
            var user = new User("Raissa", "raissa@example.com", "123456");

            // Act
            user.ChangeName("Raissa Q");

            // Assert
            user.Name.Should().Be("Raissa Q");
        }

        [Fact(DisplayName = "Given valid user When changing email Then should update successfully")]
        public void GivenValidUser_WhenChangingEmail_ThenShouldUpdateSuccessfully()
        {
            // Arrange
            var user = new User("Raissa", "raissa@example.com", "123456");

            // Act
            user.ChangeEmail("raissa.new@example.com");

            // Assert
            user.Email.Should().Be("raissa.new@example.com");
        }

        [Fact(DisplayName = "Given valid user When changing status Then should update successfully")]
        public void GivenValidUser_WhenChangingStatus_ThenShouldUpdateSuccessfully()
        {
            // Arrange
            var user = new User("Raissa", "raissa@example.com", "123456");

            // Act
            user.ChangeStatus(UserStatus.Active);

            // Assert
            user.Status.Should().Be(UserStatus.Active);
        }
        
        [Fact(DisplayName = "Given valid user When changing password Then should update successfully")]
        public void GivenValidUser_WhenChangingPassword_ThenShouldUpdateSuccessfully()
        {
            // Arrange
            var user = new User("Raissa", "raissa@example.com", "123456");

            // Act
            user.ChangePassword("newpassword");

            // Assert
            user.Password.Should().Be("newpassword");
        }
    }
}
