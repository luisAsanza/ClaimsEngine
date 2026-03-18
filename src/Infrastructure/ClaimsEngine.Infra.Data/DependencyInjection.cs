using ClaimsEngine.Infra.Data.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ClaimsEngine.Infra.Data.Persistence;
using ClaimsEngine.Infra.Data.Configuration;
using Microsoft.Extensions.Options;
using ClaimsEngine.Application.Abstractions;
using ClaimsEngine.Infra.Data.Common;
using ClaimsEngine.Infra.Data.Repositories;

namespace ClaimsEngine.Infra.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Options for Database Settings
            services.AddOptions<ClaimsDatabaseSettings>()
                .BindConfiguration(ClaimsDatabaseSettings.ClaimsDbSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // Register the interceptor as a singleton
            services.AddSingleton<InsertOutboxMessagesInterceptor>();

            // Register the DbContext with singleton lifetime and wire the interceptor
            services.AddDbContext<Persistence.ClaimDbContext>(
                (provider, options) =>
                {
                    var claimsDbSettings = provider.GetRequiredService<IOptions<ClaimsDatabaseSettings>>().Value;
                    var interceptor = provider.GetRequiredService<InsertOutboxMessagesInterceptor>();

                    options.AddInterceptors(interceptor);
                    options.UseSqlServer(
                        claimsDbSettings.ConnectionString,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(ClaimDbContext).Assembly.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: claimsDbSettings.MaxRetryCount,
                                maxRetryDelay: TimeSpan.FromSeconds(claimsDbSettings.CommandTimeoutSeconds),
                                errorNumbersToAdd: null
                            );
                        }
                    );
                });

            // Custom services registration
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbExceptionTranslator, SqlExceptionTranslator>();

            return services;
        }
    }
}
