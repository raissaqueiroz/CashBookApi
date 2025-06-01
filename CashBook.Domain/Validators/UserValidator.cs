using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashBook.Domain.Entities;
using FluentValidation;

namespace CashBook.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(entity => entity)
                .NotEmpty()
                .WithMessage("entity invalid")
                .NotNull()
                .WithMessage("entity invalid");

            RuleFor(entity => entity.Name)
                .NotEmpty()
                .WithMessage("name is required")
                .NotNull()
                .WithMessage("name is required");

            RuleFor(entity => entity.Email)
                .NotEmpty()
                .WithMessage("name is required")
                .NotNull()
                .WithMessage("name is required")
                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                .WithMessage("email is invalid");

            RuleFor(entity => entity.Password)
                .NotEmpty()
                .WithMessage("password is required")
                .NotNull()
                .WithMessage("password is required")
                .MinimumLength(6)
                .WithMessage("min. 6 characters");

            RuleFor(entity => entity.Status)
                .IsInEnum();
        }   
    }
}