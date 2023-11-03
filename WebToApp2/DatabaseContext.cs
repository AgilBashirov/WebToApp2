using Microsoft.EntityFrameworkCore;
using WebToApp2.Entities;

namespace WebToApp2;

public class DatabaseContext: DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    
    public virtual DbSet<AppFile> Files { get; set; } = null!;
}