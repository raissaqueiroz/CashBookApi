namespace CashBook.Application.Dtos;

public class LoginRequestDto(string email, string password)
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
}
