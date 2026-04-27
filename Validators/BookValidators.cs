using FluentValidation;
using Proyecto.Models;

namespace Proyecto.Validators;

public class BookValidators : AbstractValidator<Books>
{
    public BookValidators()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Book ID is required.").When(x => x.Id > 0);
        RuleFor(b => b.Name).NotEmpty().Length(2, 100).WithMessage("Book name is required.");
        RuleFor(b => b.Category).IsInEnum().WithMessage("Category is required.");
        RuleFor(x => x.Status).IsInEnum().WithMessage("El estado seleccionado no es válido.");
    }
}