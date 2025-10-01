using FOT.OrderChangedWorker.Models;

namespace FOT.OrderChangedWorker.Abstractions;

public interface IOrderNotifier
{
    ValueTask NotifyAsync(OrderStatusChangedEvent orderStatusChangedEvent);
}