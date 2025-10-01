using FOT.OutboxWorker.Abstractions;

namespace FOT.OutboxWorker;

public class Worker(IOutboxPublisher publisher, IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await publisher.PublishAsync(stoppingToken);
            await Task.Delay(configuration.GetValue<int>("WorkerDelay"), stoppingToken);
        }
    }
}