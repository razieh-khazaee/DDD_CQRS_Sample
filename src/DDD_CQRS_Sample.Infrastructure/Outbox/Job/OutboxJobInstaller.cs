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
            RecurringJob.AddOrUpdate<IProcessOutboxMessagesJob>("ProcessOutboxMessages", job => job.Execute(), config.Schedule);
        }
    }
}
