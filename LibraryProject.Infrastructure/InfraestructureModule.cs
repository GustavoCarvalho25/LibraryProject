using Core.Entities;
using Core.Repository;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfraestructureModule
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddRepositories();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<LibraryDbContext>(o => o.UseSqlServer(connectionString));
        return services;
    }
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<Book>, BookRepository>();
        
        return services;
    }
}