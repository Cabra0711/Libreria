using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;
using Proyecto.Response;
using Proyecto.Services.Interfaces;
using FluentValidation;
using Proyecto.Validators;

namespace Proyecto.Services;

public class BookService : IBookServices
{
    // ------ BOOKS SERVICE -------
    
    /*
     * LLamamos al context para pasarle los datos que vmos a menejar dentro de los lubros todo esto con el fin
     * de poder usar mas adelante metodos async
     */
    private readonly MySqlDbContext _context;
    private readonly IValidator<Books> _bookValidator;
    public BookService(MySqlDbContext context, IValidator<Books> bookValidator)
    {
        _context = context;
        _bookValidator = bookValidator;
    }
    
    /*
     * Creamos un metodo asincrono donde le vamos a decir que llame al modelo de service response
     * obtenemos los parametros por medio de un IEnumerable ya que esto nos trae una coleccion de solo lectura
     * no se puede modificar mientras corre el programa
     */
    public async Task<ServiceResponse<IEnumerable<Books>>> GetAllBooks()
    {
        // dentro de esto utilizamos var ya que no sabemos que tipo de dato nos va a retornar directamente y almacenar
        // DIciendo que dentro de esto vamos a esperar a que la base de datos nos liste todo para evitar congelaciones dentro el sistema
        var books = await _context.Books.ToListAsync();

        //retornamos el service response con los libros haciendole saber al usuario que todo fue correctamente tarido
        return new ServiceResponse<IEnumerable<Books>>()
        {
            Data = books,
            Success = true
        };
    }
    /*
     * Obtenemos libros por ID al asignar el operador de coalselcia decimos que este puede llegar a nosotros como NULL
     * para evitar que nuestro programa explote en caso de que nos llegue algo en NULL a nuestra vista
     */
    public async Task<ServiceResponse<Books?>> GetBookById(int id)
    {
        // llamamos al modelo de la respuesta 
        var response = new ServiceResponse<Books?>();
        
        // usamos await para esperar respuesta de la base de datos evitando congelaciones dentro de nustro sistema
        // usando FirstOrDefault para pasarle el parametro que quieremos que busque dentro nuestro sistema
        // esto con el fin de que nos busque un libro dentro del sistema
        var books = await _context.Books.FirstOrDefaultAsync(u => u.Id == id);

        if (books != null)
        {
            response.Data = books;
            response.Success = true;
            response.Message = "Book found";
            return response;
        }
        else
        {
            response.Success = false;
            response.Message = "Book not found";
            return response;
        }
    }
    /*
     * Aca dentro llamamos al modelo y al ID para traer los parametros dentro este modelo + el ID de este para saber que tipo de libro estamos editando
     */
    public async Task<ServiceResponse<Books>> Update( int id, Books libroEditar)
    {
        // en este caso usamos findAsync porque nos resulta mas comodo al momento de trabajar con solamente un ID 
        var response = new ServiceResponse<Books>();
        var validation = await _bookValidator.ValidateAsync(libroEditar);
        if (!validation.IsValid)
        {
            response.Success = false;
            response.Message = string.Join(" | ", validation.Errors.Select(x => x.ErrorMessage));
            return response;
        }
        var idExiste = await _context.Books.FindAsync(id);

        if(idExiste == null)
        {
            response.Success = false;
            response.Message = "There is a book with this ID";
            return response;
        }
        else
        {
            idExiste.Name = libroEditar.Name;
            idExiste.Author = libroEditar.Author;
            idExiste.Category =  libroEditar.Category;
            idExiste.Status = libroEditar.Status;
            // mandamos cambios a la DB por medio del ID y que espere nuevamente para evitar congelaciones dentro del sistema
            _context.Books.Update(idExiste);
            await _context.SaveChangesAsync();
            
            response.Success = true;
            response.Data = idExiste;
            response.Message = "Book updated";
            return response;
        }
        
    }
    // llamamos al modelo para que irterprete nuestros parametros que estamos asignando con el fin de mandar estos datos a la DB
    public async Task<ServiceResponse<Books>> CreateBook(Books libroCrear)
    {
        var response = new ServiceResponse<Books>();
        var validation = await _bookValidator.ValidateAsync(libroCrear);
        if (!validation.IsValid)
        {
            response.Success = false;
            response.Message = string.Join(" | ", validation.Errors.Select(x => x.ErrorMessage));
            return response;
        }
        // En este caso usamos FIrstOrDefault porque el tipo que nos va a devolver es de tipo STRING entonces no podemos trabajar con FInd para terminos de comodidad y que no explote el sistema
        var nombreExiste = await _context.Books.FirstOrDefaultAsync(l => l.Name == libroCrear.Name);

        if(nombreExiste != null)
        {
            response.Success = false;
            response.Message = "There is a book created with this Name!";
            return response;
        }
        else
        {
            await _context.Books.AddAsync(libroCrear);
            await _context.SaveChangesAsync();
            
            response.Success = true;
            response.Data = libroCrear;
            response.Message = "Book created";
            return response;
        }
    }
    // borrramos por indice para temas de velocidad al ser un ID UNICO este va encontrarlo de manera mas eficiente haciendo que nustro programa traiga los datos mas rapido
    public async Task<ServiceResponse<Books>> Delete(int id)
    {
        var response = new ServiceResponse<Books>();
        var idExiste = await _context.Books.FindAsync(id);

        if (idExiste != null)
        {
            //decimos que borre nuestro objeto traido en la DB
            _context.Books.Remove(idExiste);
            await _context.SaveChangesAsync();
            
            response.Success = true;
            response.Data = idExiste;
            response.Message = "Book deleted";
            return response;
        }
        else
        {
            response.Success = false;
            response.Message = "Book Not Found";
            return response;
        }
    }
}