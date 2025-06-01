using CashBook.Domain.Entities;

namespace CashBook.Infra.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmail(string email);
    Task<IEnumerable<User>> SearchByEmail(string email);
    Task<IEnumerable<User>> SearchByName(string name);
}