using CashBook.Core.Enums;
using CashBook.Domain.Validators;

namespace CashBook.Domain.Entities;

public class User : BaseEntity
{
    public User(string name, string email, string password)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Password = password;
        _errors = new List<string>();
    }
    
    protected User(){} /* EF */

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public UserStatus Status { get; private set; } = UserStatus.Unconfirmed;
    
    public override bool Validate()
    {
        var validator = new UserValidator();
        var validation = validator.Validate(this);

        if(!validation.IsValid){
            foreach (var error in validation.Errors)
                _errors.Add(error.ErrorMessage);

            throw new Exception("Invalid Fields");
        }

        return true;
    }

    public void ChangeName(string name)
    {
        Name = name;
        Validate();
    }
    
    public void ChangeEmail(string email)
    {
        Email = email;
        Validate();
    }
    
    public void ChangePassword(string password)
    {
        Password = password;
        Validate();
    }
    
    public void ChangeStatus(UserStatus status)
    {
        Status = status;
        Validate();
    }
}