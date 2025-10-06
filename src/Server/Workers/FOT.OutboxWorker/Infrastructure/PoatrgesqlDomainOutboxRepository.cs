using FOT.OutboxWorker.Abstractions;
using Npgsql;

namespace FOT.OutboxWorker.Infrastructure;

public class PoatrgesqlDomainOutboxRepository(NpgsqlConnection connection)
{
    private const string _selectEventsQuery = """
                                             SELECT "DomainEventOutboxId", "AggregateType", "AggregateId", "EventCode", "Payload", "OccurredAt", "ProcessedAt"
                                             FROM "DomainEventOutboxNotifications"
                                             WHERE "ProcessedAt"  IS NULL
                                             ORDER BY "OccurredAt"
                                             LIMIT @limit
                                             """;

    private const string _updateEventsProcessedAtCommand = """
                                             UPDATE "DomainEventOutboxNotifications"
                                             SET "ProcessedAt" = @ProcessedAt
                                             WHERE "DomainEventOutboxId" IN (@inClause)
                                             """;

    public async ValueTask<DomainEventOutbox[]> GetDomainEventsToSentAsync(int limit, CancellationToken cancellationToken)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = _selectEventsQuery;
        cmd.Parameters.AddWithValue("@limit", limit);
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        var events = new List<DomainEventOutbox>();
        while (await reader.ReadAsync(cancellationToken))
        {
            events.Add(new DomainEventOutbox()
            {
                DomainEventOutboxId = reader.GetInt32(0),
                AggregateType = reader.GetString(1),
                AggregateId = reader.GetString(2),
                EventCode = reader.GetString(3),
                Payload = reader.GetString(4),
                OccurredAt = reader.GetDateTime(5),
                ProcessedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
            });
        }
        await reader.CloseAsync();

        return events.ToArray();
    }

    public async ValueTask UpdateProcessedAt(int[] eventsIds, DateTimeOffset processedAt, CancellationToken cancellation)
    {
        var parameters = eventsIds.Select((id, i) => new NpgsqlParameter($"@id{i}", id)).ToArray();
        var inClause = string.Join(", ", parameters.Select(p => p.ParameterName));
        var cmd = connection.CreateCommand();
        cmd.CommandText = _updateEventsProcessedAtCommand.Replace("@inClause", inClause);
        cmd.Parameters.AddRange(parameters);
        cmd.Parameters.AddWithValue("@ProcessedAt", processedAt.DateTime);
        await cmd.ExecuteNonQueryAsync(cancellation);
    }
}
