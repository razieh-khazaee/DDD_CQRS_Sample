using DDD_CQRS_Sample.Infrastructure.Outbox.Settings;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.JobInstaller;

namespace DDD_CQRS_Sample.Infrastructure.Outbox.Job
{
    public class OutboxJobInstaller : IJobInstaller
    {
        public void Install(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetRequiredService<IOptions<OutboxOptions>>().Value;
            var processOutboxJob = serviceProvider.GetRequiredService<ProcessOutboxMessagesJob>();
            RecurringJob.AddOrUpdate($"ProcessOutboxMessages", () => processOutboxJob.Execute(), $"0/{config.IntervalInSeconds} * * * * *");
        }
    }
}
