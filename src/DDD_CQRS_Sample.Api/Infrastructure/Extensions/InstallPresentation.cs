using Hangfire;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace DDD_CQRS_Sample.Api.Infrastructure.Extensions
{
    public static class InstallPresentation
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddFluentValidationRulesToSwagger();
            services.AddHttpContextAccessor();

            services.AddHangfireServer();

            return services;
        }
    }
}
