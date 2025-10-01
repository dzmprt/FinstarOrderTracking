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
    private static readonly Dictionary<OrderStatusEnum, HashSet<OrderStatusEnum>> StatusTransitions = new()
    {
        [OrderStatusEnum.Created] = [OrderStatusEnum.Shipped, OrderStatusEnum.Cancelled],
        [OrderStatusEnum.Shipped] = [OrderStatusEnum.Delivered, OrderStatusEnum.Cancelled],
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
    public OrderStatusEnum Status { get; private set; }
    
    /// <summary>
    /// Created date.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }
    
    /// <summary>
    /// Las update date.
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    private Order()
    {}
    
    public Order(string description, DateTimeOffset now)
    {
        ArgumentException.ThrowIfNullOrEmpty(description, nameof(description));
        description.ThrowIfLengthOutOfRange(MinDescriptionLength, MaxDescriptionLength);
        Description = description;
        Status = OrderStatusEnum.Created;
        CreatedAt = now.UtcDateTime;
    }

    public void ChangeStatus(OrderStatusEnum newStatusEnum, DateTimeOffset now)
    {
        if (Status == newStatusEnum)
            return;

        if (!StatusTransitions.TryGetValue(Status, out var allowed) || !allowed.Contains(newStatusEnum))
            throw new InvalidOperationException($"Unable to change order status from {Status} to {newStatusEnum}");

        Status = newStatusEnum;
        UpdatedAt = now.UtcDateTime;
        AddDomainEvent(new OrderStatusChangedEvent(this));
    }
}