using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CashBook.Infra.Contexts;

public class ManagerContextFactory : IDesignTimeDbContextFactory<ManagerContext>
{
    public ManagerContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetSection("ConnectionString").Value;
        
        var optionsBuilder = new DbContextOptionsBuilder<ManagerContext>();
        optionsBuilder.UseMySql(connectionString ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not defined."), ServerVersion.AutoDetect(connectionString));

        return new ManagerContext(optionsBuilder.Options);
        
    }
}