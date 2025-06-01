using CashBook.Domain.Entities;
using CashBook.Infra.Contexts;
using CashBook.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CashBook.Infra.Repositories;

public class BaseRepository<T>(ManagerContext context) : IBaseRepository<T> where T : BaseEntity
{
    public virtual async Task<T> Create(T obj)
    {
        context.Add(obj);
        await context.SaveChangesAsync();

        return obj;
    }

    public virtual async Task<T> Update(T obj)
    {
        context.Entry(obj).State = EntityState.Modified;
        await context.SaveChangesAsync();
        
        return obj;
    }

    public virtual async Task Remove(Guid id)
    {
        var obj = await GetById(id);

        if (obj != null)
        {
            context.Remove(obj);
            await context.SaveChangesAsync();
        }
    }

    public virtual async Task<T?> GetById(Guid id)
    {
        return await context.Set<T>().AsNoTracking().Where(entity => entity.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await context.Set<T>().AsNoTracking().ToListAsync();
    }
}