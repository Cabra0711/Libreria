using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Enums;
using Proyecto.Models;
using Proyecto.Response;
using FluentValidation;
using Proyecto.Services.Interfaces;
using Proyecto.Validators;

namespace Proyecto.Services;

public class LoansService : ILoanServices
{
    private readonly IValidator<Loans> _loansValidator;
    
    private readonly MySqlDbContext _context;
    public LoansService(MySqlDbContext context, LoanValidators validator)
    {
        _context = context;
        _loansValidator = validator;
    }
    
    public async Task<ServiceResponse<IEnumerable<Loans>>> GetAllLoans()
    {
        var loans = await _context.Loans.Include(l => l.User).Include(l => l.Book).ToListAsync();
        
        return new ServiceResponse<IEnumerable<Loans>>()
        {
            Data = loans,
            Success = true
        };
    }

    public async Task<ServiceResponse<Loans?>> GetLoansById(int id)
    {
        var response = new ServiceResponse<Loans?>();
        
        var loans = await _context.Loans.FirstOrDefaultAsync(l => l.Id == id);
        if (loans != null)
        {
            response.Data = loans;
            response.Success = true;
            response.Message = "Loan found";
            return response;
        }
        else
        {
            response.Success = false;
            response.Message = "Loan not found";
            return response;
        }
    }

    public async Task<ServiceResponse<Loans>> Create(Loans prestamoCrear)
    {
        var response = new ServiceResponse<Loans>();
        var validation = await _loansValidator.ValidateAsync(prestamoCrear);
        if (!validation.IsValid)
        {
            response.Success = false;
            response.Message = string.Join(" | ", validation.Errors.Select(x => x.ErrorMessage));
            return response;
        }
        
        
        var usuarioExiste = await _context.Users.FindAsync(prestamoCrear.UserId);
        var libroExiste = await _context.Books.FindAsync(prestamoCrear.BookId);
        
        
        
        
        if (libroExiste != null && usuarioExiste != null)
        {
            response.Success = true;
            response.Message = "Usuario y libro encontrado";
            var status = libroExiste.Status;
            if (status == BookStatus.Available)
            {
                response.Data = prestamoCrear;
                response.Success = true;
                libroExiste.Status = BookStatus.Borrowed;
                _context.Loans.Add(prestamoCrear);
                await _context.SaveChangesAsync();
                return response;
            }else if (status == BookStatus.Unavailable)
            {
                response.Success = false;
                response.Message = "Loan not available";
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = "Loan can not be completed the book have been borrowed";
                return response;
            }

        }
        else
        {
            response.Success = false;
            response.Message = "Usuario y libro no encontrados";
            return response;
        }
        return response;
    }

    public async Task<ServiceResponse<Loans>> Update(Loans prestamoEditar, int id)
    {
        var response = new ServiceResponse<Loans>();
        var validation = await _loansValidator.ValidateAsync(prestamoEditar);
        if (!validation.IsValid)
        {
            response.Success = false;
            response.Message = string.Join(" | ", validation.Errors.Select(x => x.ErrorMessage));
            return response;
        }
        
        var idExiste = await _context.Loans.FindAsync(id);
        if (idExiste == null)
        {
            response.Success = false;
            response.Message = "This user does not exist";
            return response;
        }
        else
        {
            var buscarLibro = await _context.Books.FindAsync(prestamoEditar.BookId);
            if (buscarLibro != null)
            {
                idExiste.Date = prestamoEditar.Date;
                idExiste.DeliveryDate = prestamoEditar.DeliveryDate;
                buscarLibro.Status = BookStatus.Available;
                
                _context.Loans.Update(idExiste);
                await _context.SaveChangesAsync();
            
            
                response.Success = true;
                response.Data = idExiste;
                response.Message = "User updated";
                return response;
            }
        }
        return response;
    }
    
    
}