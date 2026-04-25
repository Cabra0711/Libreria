using FluentValidation;
using Proyecto.Data;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Validators;

public class LoanValidators : AbstractValidator<Loans>
{
    public LoanValidators()
    {
        RuleFor(l => l.Id).NotNull();
        RuleFor(l => l.Date).NotNull().GreaterThanOrEqualTo(DateTime.Today).WithMessage("No se permiten reservar libros para otros dias");
        RuleFor(l => l.DeliveryDate).NotEmpty().GreaterThan(l => l.Date);
        RuleFor(l => l.BookId).GreaterThan(0).NotNull();
        RuleFor(l => l.UserId).GreaterThan(0).NotNull();
    }
}