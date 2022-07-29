using FluentValidation;
using Hays.Domain.Entities;
using Hays.Domain.Models;

namespace Hays.Domain.Validators
{
    public class CustomerValidator : AbstractValidator<Customers>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                .Length(2, 100)
                .WithMessage(Const.NameSize);

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage(Const.EmailSize);

            RuleFor(x => x.Surname)
                .Length(2, 100)
                .WithMessage(Const.SruenameSize);

            RuleFor(x => x.Password)
                .Length(4, 10)
                .WithMessage(Const.PasswordSize);
        }
    }
}
