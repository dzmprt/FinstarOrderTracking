using FOT.Domain.Common;
using FOT.Domain.Orders.Enums;

namespace FOT.Domain.Orders.Events;

/// <summary>
/// Order status changed event.
/// </summary>
public sealed class OrderStatusChangedEvent : BaseDomainEvent
{
    /// <summary>
    /// Order number.
    /// </summary>
    public Guid OrderNumber { get; }

    /// <summary>
    /// Status.
    /// </summary>
    public OrderStatus NewStatus { get; private set; }

    /// <summary>
    /// Las update date.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderStatusChangedEvent"/>.
    /// </summary>
    /// <param name="order">State of order.</param>
    public OrderStatusChangedEvent(Order order) : base(DateTimeOffset.UtcNow)
    {
        OrderNumber = order.OrderNumber;
        NewStatus = order.Status;
        UpdatedAt = order.UpdatedAt!.Value;
    }
}
