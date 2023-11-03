using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static System.Boolean;

namespace WebToApp2;

public static class DataAccessDependencyInjection
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseConfiguration databaseConfig = new()
        {
            ConnectionString = configuration["Database:ConnectionStrings"],
            UseInMemoryDatabase = Parse(configuration["Database:UseInMemoryDatabase"]!)
        };

        if (databaseConfig.UseInMemoryDatabase)
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseInMemoryDatabase("Dev");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        else
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(databaseConfig.ConnectionString,
                    opt => opt.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)));
    }
}
public class DatabaseConfiguration
{
    public string? ConnectionString { get; init; }
    public bool UseInMemoryDatabase { get; init; }
}