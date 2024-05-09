namespace DDD_CQRS_Sample.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    private OutboxMessage()
    {

    }

    public OutboxMessage(DateTime occurredOnUtc, string type, string content)
    {
        OccurredOn = occurredOnUtc;
        Content = content;
        Type = type;
    }

    public Guid Id { get; init; }

    public DateTime OccurredOn { get; init; }

    public string Type { get; init; }

    public string Content { get; init; }

    public DateTime? ProcessedOn { get; init; }

    public string? Error { get; init; }
}
