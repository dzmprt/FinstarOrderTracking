using System.Text.Json;
using FOT.OrderChangedWorker.Abstractions;
using FOT.OrderChangedWorker.Models;

namespace FOT.OrderChangedWorker.Infrastructure;

public class KafkaMessageHandler(IOrderNotifier notifier, ILogger<KafkaMessageHandler> logger)
{
    public async Task HandleAsync(string message)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var orderUpdate = JsonSerializer.Deserialize<OrderStatusChangedEvent>(message, options);
            if (orderUpdate != null)
            {
                logger.LogInformation("Received order state: {OrderId}: {state}", orderUpdate.OrderNumber, orderUpdate);
                await notifier.NotifyAsync(orderUpdate);
            }
            else
            {
                logger.LogWarning("Failed to deserialize order update: {Message}", message);
            }
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "JSON deserialization error for message: {Message}", message);
        }
    }
}