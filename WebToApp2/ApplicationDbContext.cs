using Microsoft.EntityFrameworkCore;
using WebToApp2.Entities;

namespace WebToApp2;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<AppFile> Files { get; set; } = null!;
    public virtual DbSet<Operation> Operations { get; set; } = null!;
    public virtual DbSet<OperationFile> OperationFiles { get; set; } = null!;

}