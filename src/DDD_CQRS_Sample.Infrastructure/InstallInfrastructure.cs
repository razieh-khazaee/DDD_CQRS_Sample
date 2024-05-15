using DDD_CQRS_Sample.Infrastructure.Data;
using DDD_CQRS_Sample.Infrastructure.DbContexts;
using DDD_CQRS_Sample.Infrastructure.Email;
using DDD_CQRS_Sample.Infrastructure.Outbox.Job;
using DDD_CQRS_Sample.Infrastructure.Outbox.Settings;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Shared.Data;

namespace DDD_CQRS_Sample.Infrastructure;

public static class InstallInfrastructure
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        #region DbContext

        string dbConnectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<DDD_CQRS_SampleDbContext>(options => options.UseSqlServer(dbConnectionString));
        services.AddScoped<IUnitOfWork>(m => m.GetRequiredService<DDD_CQRS_SampleDbContext>());
        services.AddSingleton<ISqlConnectionFactory>(m => new SqlConnectionFactory(dbConnectionString));

        #endregion

        #region Email

        services.Configure<EmailSettingOptions>(m => configuration.GetSection("EmailSettings").Bind(m));

        #endregion

        #region Hangfire

        string hangfireConnectionString = configuration.GetConnectionString("Hangfire") ?? throw new ArgumentNullException(nameof(configuration));
        services.AddHangfire(m =>
        {
            m.UseSqlServerStorage(hangfireConnectionString);
            m.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
        });
        services.AddHangfireServer(m =>
        {
            m.SchedulePollingInterval = TimeSpan.FromSeconds(1);
        });

        #endregion

        #region Outbox

        services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesJob>();
        services.AddOptions<OutboxOptions>()
               .Bind(configuration.GetSection("Outbox"))
               .ValidateDataAnnotations()
               .ValidateOnStart();

        #endregion

        #region Dependency Injection-Must be after other dependeny injections

        services.Scan(selector => selector
              .FromAssemblies(typeof(InstallInfrastructure).Assembly)
              .AddClasses(publicOnly: false)
              .UsingRegistrationStrategy(RegistrationStrategy.Skip)
              .AsMatchingInterface()
              .WithScopedLifetime());

        #endregion

        return services;
    }
}