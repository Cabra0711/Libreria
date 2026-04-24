namespace Proyecto.Models;

public class Loans : BaseEntity
{
    public int UserId { get; set; }
    public Users User { get; set; }
    public int BookId { get; set; }
    public Books Book { get; set; }
    public DateTime Date { get; set; }
    public DateTime DeliveryDate { get; set; }
}