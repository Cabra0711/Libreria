using Proyecto.Enums;

namespace Proyecto.Models;

public class Books : BaseEntity
{
    public string Name { get; set; }
    public string Author { get; set; }
    public BookEnums Category { get; set; }
    public BookStatus Status { get; set; }
    public ICollection<Loans> Loans { get; set; }
}