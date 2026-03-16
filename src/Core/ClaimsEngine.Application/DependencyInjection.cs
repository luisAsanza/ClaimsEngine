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

            // Register Pipeline Behaviors
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        });
        return services;
    }
}
