using CashBook.Core.Enums;

namespace CashBook.Application.Dtos;

public class UserReadDto(Guid id, string name, string email, UserStatus status)
{
    public Guid Id { get; set; } = id;
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public UserStatus Status { get; set; } = status;
}