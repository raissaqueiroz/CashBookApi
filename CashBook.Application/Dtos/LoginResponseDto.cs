namespace CashBook.Application.Dtos;

public class LoginResponseDto(string token, DateTime expiresAt)
{
    public string Token { get; private set; } = token;
    public DateTime ExpiresAt { get; private set; } = expiresAt;
}
