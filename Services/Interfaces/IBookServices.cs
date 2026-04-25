using Proyecto.Models;
using Proyecto.Response;

namespace Proyecto.Services.Interfaces;

public interface IBookServices
{
    public Task<ServiceResponse<IEnumerable<Books>>> GetAllBooks();
    public Task<ServiceResponse<Books?>> GetBookById(int id);
    public Task<ServiceResponse<Books>> Create(Books libroCrear);
    public Task<ServiceResponse<Books>> Update(Books libroEditar, int id);
    public Task<ServiceResponse<Books>> Delete(int id);
}