using CashBook.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using CashBook.Domain.Entities;

namespace CashBook.Core.Services;

public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher;

    public PasswordHasherService()
    {
        _passwordHasher = new PasswordHasher<User>();
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public PasswordVerificationResult VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
    }
}