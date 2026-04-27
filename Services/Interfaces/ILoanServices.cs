using Proyecto.Models;
using Proyecto.Response;

namespace Proyecto.Services.Interfaces;

public interface ILoanServices
{
    public Task<ServiceResponse<IEnumerable<Loans>>> GetAllLoans();
    public Task<ServiceResponse<Loans>> CreateLoan(Loans prestamoCrear);
    public Task<ServiceResponse<Loans?>> GetLoansById(int id);
    public Task<ServiceResponse<Loans>> Update(Loans prestamoEditar, int id);
    public Task<ServiceResponse<Loans>> ReturnLoan(int id);
}