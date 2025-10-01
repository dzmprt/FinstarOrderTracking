namespace FOT.OrderChangedWorker.Models;

/// <summary>
/// Order.
/// </summary>
public sealed class OrderStatusChangedEvent
{
    /// <summary>
    /// Order number.
    /// </summary>
    public Guid OrderNumber { get; init; }
    
    /// <summary>
    /// Status.
    /// </summary>
    public OrderStatusEnum NewStatus { get; init; }
    
    /// <summary>
    /// Las update date.
    /// </summary>
    public DateTime UpdatedAt { get; init; }
}