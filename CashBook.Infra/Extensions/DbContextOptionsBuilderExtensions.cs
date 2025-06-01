using CashBook.Domain.Entities;
using CashBook.Infra.Configuration;
using CashBook.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashBook.Infra.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static void AddManagerContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ManagerContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 26)))
        );
    }
    
    public static void AddEntityConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IEntityTypeConfiguration<User>, UserConfiguration>();
    }
}