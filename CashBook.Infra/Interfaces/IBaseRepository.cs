using CashBook.Domain.Entities;

namespace CashBook.Infra.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> Create(T obj);
    Task<T> Update(T obj);
    Task Remove(Guid id);
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll();
}