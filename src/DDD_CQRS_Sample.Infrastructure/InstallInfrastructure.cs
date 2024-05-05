using DDD_CQRS_Sample.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Shared.Audit;
using Shared.DbContexts;

namespace DDD_CQRS_Sample.Infrastructure;

public static class InstallInfrastructure
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database") ??
                                 throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<DDD_CQRS_SampleDbContext>(options => options
                    .UseSqlServer(connectionString)
                    .AddInterceptors(new FillAuditFieldsInterceptor()));

        //dependency injection
        services.Scan(selector => selector
                .FromAssemblies(typeof(InstallInfrastructure).Assembly)
                .AddClasses(publicOnly: false)
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithScopedLifetime());

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<DDD_CQRS_SampleDbContext>());

        return services;
    }
}