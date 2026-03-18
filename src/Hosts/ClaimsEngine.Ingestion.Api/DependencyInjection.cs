using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace ClaimsEngine.Ingestion.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddControllers();

        return services;
    }
}
