using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfraestructureModule
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<LibraryDbContext>(o => o.UseSqlServer(connectionString));
        return services;
    }
}