using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.MediatR.Behaviors;

namespace DDD_CQRS_Sample.Application;

public static class InstallApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(InstallApplication).Assembly);

            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(InstallApplication).Assembly, includeInternalTypes: true);

        return services;
    }
}