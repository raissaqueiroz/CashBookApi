using CashBook.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CashBook.Application.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(User user, string password);
    PasswordVerificationResult VerifyPassword(User user, string hashedPassword, string providedPassword);
}