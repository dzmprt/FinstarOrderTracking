using System.Text.Json;
using System.Diagnostics;
using FOT.OrderChangedWorker.Abstractions;
using FOT.OrderChangedWorker.Models;

namespace FOT.OrderChangedWorker.Infrastructure;

public class WebSocketOrderNotifier(WebSocketConnectionManager manager, ILogger<WebSocketOrderNotifier> logger)
    : IOrderNotifier
{
    public async ValueTask NotifyAsync(OrderStatusChangedEvent orderStatusChangedEvent)
    {
        using var activity = new ActivitySource("FOT.OrderChangedWorker.WebSocket").StartActivity("Broadcast OrderUpdate");
        var message = JsonSerializer.Serialize(orderStatusChangedEvent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        activity?.SetTag("websocket.broadcast.length", message.Length);
        activity?.SetTag("order.id", orderStatusChangedEvent.OrderNumber);
        logger.LogInformation("Broadcast order state: {message}", message);
        await manager.BroadcastAsync(message);
    }
}