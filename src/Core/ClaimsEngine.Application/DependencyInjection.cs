using ClaimsEngine.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ClaimsEngine.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
            cfg.Lifetime = ServiceLifetime.Scoped; // Set MediatR services to Scoped lifetime

            // Register Pipeline Behaviors
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        });
        return services;
    }
}
