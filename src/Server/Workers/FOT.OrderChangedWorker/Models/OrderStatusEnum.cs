namespace FOT.OrderChangedWorker.Models;

/// <summary>
/// Order status.
/// </summary>
public enum OrderStatusEnum
{
    Created = 1,
    Shipped = 2,
    Delivered = 3,
    Cancelled = 4
}