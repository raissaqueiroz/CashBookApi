using CashBook.Domain.Entities;
using CashBook.Infra.Contexts;
using CashBook.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CashBook.Infra.Repositories;

public class UserRepository(ManagerContext context) : BaseRepository<User>(context), IUserRepository 
{
    public async Task<User?> GetByEmail(string email)
    {
        return await context.Users.Where(entity => entity.Email.ToLower() == email.ToLower()).AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> SearchByEmail(string email)
    {
        return await context.Users.Where(entity => entity.Email.ToLower().Contains(email.ToLower())).AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> SearchByName(string name)
    {
        return await context.Users.Where(entity => entity.Name.ToLower().Contains(name.ToLower())).AsNoTracking()
            .ToListAsync();
    }
}