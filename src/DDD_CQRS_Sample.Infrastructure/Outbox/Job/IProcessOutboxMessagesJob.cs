
namespace DDD_CQRS_Sample.Infrastructure.Outbox.Job
{
    public interface IProcessOutboxMessagesJob
    {
        Task Execute();
    }
}