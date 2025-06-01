namespace CashBook.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    
    /* Toda entidade precisa se auto validar. Isso evita necessidade de multiplas exceções */
    internal List<string> _errors;

    public IReadOnlyCollection<string> Errors => _errors;
    public abstract bool Validate();
}