namespace DDD_CQRS_Sample.Infrastructure.Outbox.Job;

internal sealed record OutboxMessageResponse(Guid Id, string Content);
