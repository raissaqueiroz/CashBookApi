using CashBook.Application.Dtos;
using CashBook.Application.Interfaces;
using CashBook.Application.Settings;
using CashBook.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CashBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        LoginRequestDto request,
        [FromServices] IAuthService authService,
        [FromServices] IOptions<JwtSettings> jwtSettings)
    {
        try
        {
            var token = await authService.Authenticate(request.Email, request.Password);
            var expiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.Value.ExpirationInMinutes);

            return Ok(new LoginResponseDto(token, expiresAt));
        }
        catch (DomainException)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }
    }
}
