using DDD_CQRS_Sample.Api.Infrastructure.Extensions;
using DDD_CQRS_Sample.Application;
using DDD_CQRS_Sample.Infrastructure;
using Hangfire;
using Shared.JobInstaller;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Environment);

WebApplication app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{

});

#region Run JobInstallers

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
    var moduleInstallers = assemblies.SelectMany(m => m.DefinedTypes).Where(type => typeof(IJobInstaller)
                                                .IsAssignableFrom(type) &&
                                                !type.IsInterface &&
                                                !type.IsAbstract)
                        .Select(Activator.CreateInstance)
                        .Cast<IJobInstaller>()
                        .ToList();

    moduleInstallers.ForEach(m => m.Install(services));
}
#endregion


app.Run();


