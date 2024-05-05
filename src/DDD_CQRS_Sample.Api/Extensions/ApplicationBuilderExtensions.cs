using DDD_CQRS_Sample.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
namespace DDD_CQRS_Sample.Api.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using DDD_CQRS_SampleDbContext dbContext = scope.ServiceProvider.GetRequiredService<DDD_CQRS_SampleDbContext>();

        dbContext.Database.Migrate();
    }
}
