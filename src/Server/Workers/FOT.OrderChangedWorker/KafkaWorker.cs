using FOT.OrderChangedWorker.Infrastructure;

namespace FOT.OrderChangedWorker;

public class KafkaWorker(
    ILogger<KafkaWorker> logger,
    KafkaConsumerFactory consumerFactory,
    KafkaMessageHandler messageHandler,
    IConfiguration configuration)
    : BackgroundService
{
    private readonly string _topic = configuration["Kafka:OrderStateTopic"] ??
                                     throw new InvalidOperationException("Kafka topic is not configured.");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("KafkaWorker started. Subscribed to topic: " + _topic);

        using var consumer = consumerFactory.CreateConsumer();
        consumer.Subscribe(_topic);

        await Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation($"Waiting for a message from topic '{consumer.Subscription.Single()}'...");
                    var result = consumer.Consume(stoppingToken);
                    logger.LogInformation($"Received a message: {result.Message.Key}:{result.Message.Value}");

                    await messageHandler.HandleAsync(result.Message.Value);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Kafka consume error");
                }

                await Task.Delay(configuration.GetValue<int>("WorkerDelay"), stoppingToken);
            }
        }, stoppingToken);


        consumer.Close();
    }
}