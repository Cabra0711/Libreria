using FluentValidation;
using Proyecto.Models;

namespace Proyecto.Validators;

public class BookValidators : AbstractValidator<Books>
{
    public BookValidators()
    {
        RuleFor(b => b.Id).NotEmpty().WithMessage("Book ID is required.");
        RuleFor(b => b.Name).NotEmpty().Length(2, 100).WithMessage("Book name is required.");
        RuleFor(b => b.Category).NotEmpty().IsInEnum().WithMessage("Category is required.");
        RuleFor(b => b.Status).NotEmpty().IsInEnum().WithMessage("Status is required.");
    }
}