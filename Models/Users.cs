using Proyecto.Enums;

namespace Proyecto.Models;

public class Users : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
 
    public UserEnums Status { get; set; }
    // Bolsillo dinamico que se establece para hacer la relacion entre las tablas permite flexibilidad entre las tablas para poder modificarlas en futuros
    public ICollection<Loans> Loans { get; set; }
}
