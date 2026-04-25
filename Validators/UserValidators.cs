using FluentValidation;
using Proyecto.Models;

namespace Proyecto.Validators;

public class UserValidators : AbstractValidator<Users>
{
    public UserValidators()
    {
        RuleFor(u => u.Id).NotNull();
        RuleFor(u => u.Name).NotEmpty().MinimumLength(3).WithMessage("The name is required.");
        RuleFor(u => u.LastName).NotEmpty().MinimumLength(4).WithMessage("Lastname is required.");
        RuleFor(u => u.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
    }
}