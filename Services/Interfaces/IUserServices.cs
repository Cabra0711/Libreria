using Proyecto.Models;
using Proyecto.Response;

namespace Proyecto.Services.Interfaces;

public interface IUserServices
{
    // ----USUARIOS ------
    
    // Traemos todos los usuarios con un IEnumerable para que lleguen uno por uno
    public Task<ServiceResponse<IEnumerable<Users>>> GetAllUsers();
    //Asignamos un Task para indicar que la operacion sera asincrona dentro de la capa servicios y asignamos que busque por ID entonces no hay
    //necesidad de poner IEnumerable porque ya lo estamos trayendo por ID
    public Task<ServiceResponse<Users?>> GetUserById(int id);
    // Llamamos al modelo que vamos a Editar y le ponemos el Service Response para que trabaje con estos parametros y asi con el resto
    // ponemos ID para que el forntend no tenga problemas al momento de traer todos estos parametros y se quede congelado volvemos la pagina mas eficiente
    public Task<ServiceResponse<Users>> Update(Users userEditar, int id);
    public Task<ServiceResponse<Users>> Create(Users userCrear);
    public Task<ServiceResponse<Users?>> Delete(int id);
}