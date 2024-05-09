using System.ComponentModel.DataAnnotations;

namespace DDD_CQRS_Sample.Infrastructure.Outbox.Settings;

public class OutboxOptions
{
    [Range(5, 100)]
    public int IntervalInSeconds { get; init; }

    [Range(1, int.MaxValue)]
    public int BatchSize { get; init; }
}
