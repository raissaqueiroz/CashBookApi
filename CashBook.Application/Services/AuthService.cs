using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CashBook.Application.Interfaces;
using CashBook.Application.Settings;
using CashBook.Core.Exceptions;
using CashBook.Infra.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CashBook.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IOptions<JwtSettings> jwtSettings) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<string> Authenticate(string email, string password)
    {
        var user = await userRepository.GetByEmail(email);

        if (user == null)
            throw new DomainException("Invalid credentials.");

        var verificationResult = passwordHasher.VerifyPassword(user, user.Password, password);

        if (verificationResult == PasswordVerificationResult.Failed)
            throw new DomainException("Invalid credentials.");

        return GenerateToken(user.Id, user.Name, user.Email);
    }

    private string GenerateToken(Guid userId, string name, string email)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Name, name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
