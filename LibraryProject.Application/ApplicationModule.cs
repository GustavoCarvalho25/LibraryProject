using Application.Commands.Books;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHandlers()
            .AddValidations();
        
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
}