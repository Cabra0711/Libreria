using Proyecto.Models;

namespace Proyecto.Services.Interfaces;

public interface ILoanServices
{
    public Task<ServiceResponse<IEnumerable<Loans>>> GetAllLoan();
    public Task<ServiceResponse<Loans>> Create(Loans prestamoCrear);
    public Task<ServiceResponse<Loans>> Update(Loans prestamoEditar, int id);
    public Task<ServiceResponse<Loans>> Delete(int id);
    public Task<Loans?> GetBookById(int id);
}