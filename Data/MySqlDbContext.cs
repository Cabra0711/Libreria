using Microsoft.EntityFrameworkCore;
using Proyecto.Models;

namespace Proyecto.Data;

public class MySqlDbContext : DbContext
{
    // fluentApi para definir nuestras relaciones dentro de la base de datos al momento de asignar foreign keys este valida que si esten las relaciones antes de pasar los datos
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //usamos fluentapi para establecer las relaciones entre DB este asigna las reglas y busca entre la relacion 1:N entre su Id de relacion FK en las tablas con las que estamos trabajando
        modelBuilder.Entity<Loans>().HasOne(b => b.Book).WithMany(b => b.Loans).HasForeignKey(b => b.BookId);
        modelBuilder.Entity<Loans>().HasOne(b => b.User).WithMany(b => b.Loans).HasForeignKey(b => b.UserId);
    }
    public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
    {
         
    }
    
    public DbSet<Users> Users { get; set; }
    public DbSet<Books> Books { get; set; }
    public DbSet<Loans> Loans { get; set; }
}