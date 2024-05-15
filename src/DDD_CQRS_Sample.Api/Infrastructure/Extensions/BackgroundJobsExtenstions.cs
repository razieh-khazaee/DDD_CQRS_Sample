using Shared.JobInstaller;

namespace DDD_CQRS_Sample.Api.Infrastructure.Extensions
{
    public static class BackgroundJobsExtenstions
    {
        public static IApplicationBuilder UseBackgroundJobs(this WebApplication app)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var moduleInstallers = assemblies.SelectMany(m => m.DefinedTypes).Where(type => typeof(IJobInstaller)
                                                        .IsAssignableFrom(type) &&
                                                        !type.IsInterface &&
                                                        !type.IsAbstract)
                                .Select(Activator.CreateInstance)
                                .Cast<IJobInstaller>()
                                .ToList();

            moduleInstallers.ForEach(m => m.Install(app.Services));

            return app;
        }
    }
}
