using Confluent.Kafka;
using FOT.OutboxWorker.Abstractions;
using Npgsql;
using System.Diagnostics;

namespace FOT.OutboxWorker.Infrastructure;

public class KafkaOutboxPublisher(
    IProducer<string, string> producer,
    IConfiguration config,
    ILogger<KafkaOutboxPublisher> logger)
    : IOutboxPublisher
{
    public async Task PublishAsync(CancellationToken cancellationToken)
    {
        using var rootActivity = new ActivitySource("FOT.OutboxWorker").StartActivity("Publish Outbox Events");
        await using var connection = new NpgsqlConnection(config.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync(cancellationToken);

        var repository = new PoatrgesqlDomainOutboxRepository(connection);
        var events = await repository.GetDomainEventsToSentAsync(100, cancellationToken);
        if (events.Length == 0)
        {
            logger.LogInformation("No events to produce");
            return;
        }

        await using var transaction = await connection.BeginTransactionAsync(cancellationToken); 
        producer.BeginTransaction();

        try
        {
            foreach (var ev in events)
            {
                using var evtActivity = new ActivitySource("FOT.OutboxWorker").StartActivity("Kafka Produce");
                evtActivity?.SetTag("messaging.system", "kafka");
                evtActivity?.SetTag("messaging.destination", ev.EventCode);
                evtActivity?.SetTag("message.key", ev.DomainEventOutboxId.ToString());
                producer.Produce(ev.EventCode, new Message<string, string>
                {
                    Key = ev.DomainEventOutboxId.ToString(),
                    Value = ev.Payload
                });
            }
            
            await repository.UpdateProcessedAt(events.Select(e => e.DomainEventOutboxId).ToArray(), DateTimeOffset.UtcNow, cancellationToken);
            producer.CommitTransaction();
            await transaction.CommitAsync(cancellationToken);

            foreach (var ev in events)
            {
                logger.LogInformation("Published {Type} event {Code} with ID {Id}", ev.AggregateType, ev.EventCode, ev.DomainEventOutboxId);
            }
        }
        catch (Exception ex)
        {
            producer.AbortTransaction();
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(ex, "Failed to publish outbox events");
        }
    }
}