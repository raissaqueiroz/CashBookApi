namespace CashBook.Application.Dtos;

public class UserUpdateDto(Guid id, string name, string email, string password)
{
    public Guid Id { get; set; } = id;
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public string Password { get; set; } = password;
}