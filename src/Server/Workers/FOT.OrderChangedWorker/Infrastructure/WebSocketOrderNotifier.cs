using System.Text.Json;
using FOT.OrderChangedWorker.Abstractions;
using FOT.OrderChangedWorker.Models;

namespace FOT.OrderChangedWorker.Infrastructure;

public class WebSocketOrderNotifier(WebSocketConnectionManager manager, ILogger<WebSocketOrderNotifier> logger)
    : IOrderNotifier
{
    public async ValueTask NotifyAsync(OrderStatusChangedEvent orderStatusChangedEvent)
    {
        var message = JsonSerializer.Serialize(orderStatusChangedEvent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        logger.LogInformation("Broadcast order state: {message}", message);
        await manager.BroadcastAsync(message);
    }
}