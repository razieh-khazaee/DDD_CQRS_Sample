using System.ComponentModel.DataAnnotations;

namespace DDD_CQRS_Sample.Infrastructure.Outbox.Settings;

public class OutboxOptions
{
    public string Schedule { get; init; }

    [Range(1, int.MaxValue)]
    public int BatchSize { get; init; }
}
