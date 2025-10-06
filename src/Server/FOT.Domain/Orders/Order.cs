using FOT.Domain.Common;
using FOT.Domain.Common.Abstractions;
using FOT.Domain.Common.Extensions;
using FOT.Domain.Orders.Enums;
using FOT.Domain.Orders.Events;

namespace FOT.Domain.Orders;

/// <summary>
/// Order.
/// </summary>
public sealed class Order : BaseDomain, IAggregateRoot
{
    #region Domain rules

    /// <summary>
    /// Order status transitions.
    /// </summary>
    private static readonly Dictionary<OrderStatus, HashSet<OrderStatus>> _statusTransitions = new()
    {
        [OrderStatus.Created] = [OrderStatus.Shipped, OrderStatus.Cancelled],
        [OrderStatus.Shipped] = [OrderStatus.Delivered, OrderStatus.Cancelled],
    };

    /// <summary>
    /// Min description length.
    /// </summary>
    public const int MinDescriptionLength = 1;

    /// <summary>
    /// Max description length.
    /// </summary>
    public const int MaxDescriptionLength = 1000;

    #endregion

    /// <summary>
    /// Order number.
    /// </summary>
    public Guid OrderNumber { get; }

    /// <summary>
    /// Description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Status.
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// Created date.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Las update date.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    private Order()
    {}

    public Order(string description, DateTimeOffset now)
    {
        ArgumentException.ThrowIfNullOrEmpty(description, nameof(description));
        description.ThrowIfLengthOutOfRange(MinDescriptionLength, MaxDescriptionLength);
        Description = description;
        Status = OrderStatus.Created;
        CreatedAt = now.UtcDateTime;
    }

    public void ChangeStatus(OrderStatus newStatus, DateTimeOffset now)
    {
        if (Status == newStatus)
        {
            return;
        }

        if (!_statusTransitions.TryGetValue(Status, out var allowed) || !allowed.Contains(newStatus))
        {
            throw new InvalidOperationException($"Unable to change order status from {Status} to {newStatus}");
        }

        Status = newStatus;
        UpdatedAt = now.UtcDateTime;
        AddDomainEvent(new OrderStatusChangedEvent(this));
    }
}
