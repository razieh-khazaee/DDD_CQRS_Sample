using Dapper;
using DDD_CQRS_Sample.Infrastructure.Outbox.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Data;
using Shared.Entities;
using System.Data;

namespace DDD_CQRS_Sample.Infrastructure.Outbox.Job;

public class ProcessOutboxMessagesJob
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IPublisher publisher,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _logger = logger;
        _outboxOptions = outboxOptions.Value;
    }

    public async Task Execute()
    {
        using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
        using IDbTransaction transaction = connection.BeginTransaction();

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, OutboxSerilizerSettings.JsonSerializerSettings)!;//do not use System.Text.Json

                await _publisher.Publish(domainEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while processing outbox message {MessageId}", outboxMessage.Id);

                exception = ex;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        transaction.Commit();
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        string sql = $"""
                      SELECT top {_outboxOptions.BatchSize} id, content
                      FROM OutboxMessages
                      WHERE ProcessedOn IS NULL
                      ORDER BY OccurredOn
                      """;

        IEnumerable<OutboxMessageResponse> outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        const string sql = @"
            UPDATE OutboxMessages
            SET ProcessedOn = @ProcessedOnUtc,error = @Error
            WHERE id = @Id";

        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = DateTime.Now,
                Error = exception?.ToString()
            },
            transaction: transaction);
    }
}
