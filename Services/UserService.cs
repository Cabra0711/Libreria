using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;
using Proyecto.Services.Interfaces;

namespace Proyecto.Services;

public class UserService : IUserServices
{
    // creamos un metodo privado para que mientras el programa este en ejecucion este no puede ser editado
    // donde vamos a traer los parametros del db context
    private readonly MySqlDbContext _context;

    public UserService(MySqlDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<IEnumerable<Users>>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        return new ServiceResponse<IEnumerable<Users>>
        {
            Data = users,
            Success = true,
        };
    }
    //creamos una funcion async para que este le mande una respuesta al usuario al momento que 
    // encuentre la data en la base de datos 
    public async Task<ServiceResponse<Users?>> GetUserById(int id)
    {
        //llamamos a la respuesta de la db que contiene los parametros del modelo y le pasamos
        // nuestros modelos dentro de este
        var response = new ServiceResponse<Users>();
        
        //que nos traiga usuario por ID pero que espere hasta que este se encuentre antes de ejecutarse
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        // si el usuario es diferente de vacio el va a mostrar un mensaje de exito
        if (user != null)
        {
            response.Success = true;
            //llamamos a la data para que nos traiga el usuario dentro de esto la cartica
            response.Data = user;
            response.Message = "User found";
            return response;
        }
        // si no que no fue encontrado  que retorne la respuesta que no fue encontrada
        else
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }
    }
    
    public async Task<ServiceResponse<Users>> Update(Users userEditar, int id)
    {
        var response = new ServiceResponse<Users>();
        var idExiste = await _context.Users.FindAsync(userEditar.Id);

        if (idExiste == null)
        {
            response.Success = false;
            response.Message = "User Not Found";
            return response;
        }
        else
        {
            idExiste.Name = userEditar.Name;
            idExiste.Email = userEditar.Email;
            idExiste.Status = userEditar.Status;
            
            _context.Users.Update(idExiste);
            await _context.SaveChangesAsync();
            
            
            response.Success = true;
            response.Data = idExiste;
            response.Message = "User updated";
            return response;
        }
    }

    public async Task<ServiceResponse<Users>> Create(Users userCrear)
    {
        var response = new ServiceResponse<Users>();
        var correoExiste = await _context.Users.FirstOrDefaultAsync(e => e.Email == userCrear.Email);

        if (correoExiste != null)
        {
            response.Success = false;
            response.Message = "User already exists";
            return response;
        }
        else
        {
            await _context.Users.AddAsync(userCrear);
            await _context.SaveChangesAsync();
            response.Success = true;
            response.Message = "User created";
            response.Data = userCrear;
            return response;
        }
    }

    public async Task<ServiceResponse<Users?>> Delete(Users userBorrar, int id)
    {
        var response = new ServiceResponse<Users>();
        var userExits = await _context.Users.FindAsync(userBorrar.Id);

        if (userExits == null)
        {
            response.Success = false;
            response.Message = "User Not Found";
            return response;
        }
        else
        {
            _context.Users.Remove(userExits);
            await _context.SaveChangesAsync();
            
            response.Data = userExits;
            response.Success = true;
            response.Message = "User deleted";
            return response;
        }
    }
    
}