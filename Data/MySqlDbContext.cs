using Microsoft.EntityFrameworkCore;
using Proyecto.Models;

namespace Proyecto.Data;

public class MySqlDbContext : DbContext
{
    public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
    {
        
    }
    public DbSet<Users> Users { get; set; }
}