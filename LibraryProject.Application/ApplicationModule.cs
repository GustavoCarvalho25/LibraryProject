using Application.Commands.Books;
using Application.Commands.Books.AddBookCommand;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHandlers()
            .AddValidations()
            .AddMappings();
        
        return services;
    }
    
    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddMediatR(config => 
            config.RegisterServicesFromAssemblyContaining<AddBookCommand>());
        
        return services;
    }

    private static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<AddBookCommandValidation>();
        
        return services;
    }
    
    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationModule).Assembly);
        
        return services;
    }
}