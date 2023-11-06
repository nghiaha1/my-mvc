using Microsoft.EntityFrameworkCore;

namespace MyMvc.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Student>? Students { get; set; }
    public DbSet<Class>? Classes { get; set; }
}