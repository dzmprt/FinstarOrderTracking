namespace FOT.OutboxWorker.Abstractions;

public interface IOutboxPublisher
{
    Task PublishAsync(CancellationToken cancellationToken);
}