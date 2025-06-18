using CashBook.Core.Enums;

namespace CashBook.Application.Dtos;

public class UserCreateDto(string name, string email, string password)
{
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public string Password { get; set; } = password;
}