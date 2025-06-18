using CashBook.Application.Dtos;

namespace CashBook.Application.Interfaces;

public interface IUserService
{
    Task Create(UserCreateDto user);
    Task<UserReadDto> Update(UserUpdateDto user);
    Task<IEnumerable<UserReadDto>> Get();
    Task<UserReadDto> Get(Guid id);
    Task<IEnumerable<UserReadDto>> SearchByEmail(string email);
    Task<IEnumerable<UserReadDto>> SearchByName(string name);
    Task<IEnumerable<UserReadDto>> GetByEmailAndPassword(string email, string password);
    Task Remove(Guid id);
}