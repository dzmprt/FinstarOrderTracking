using System.ComponentModel.DataAnnotations;
using FOT.Domain.Orders;
using FOT.Domain.Orders.Enums;

namespace FOT.Application.UseCase.Orders;

public class OrderDto(Order order)
{
    /// <summary>
    /// Order number.
    /// </summary>
    public Guid OrderNumber { get; set; } = order.OrderNumber;

    /// <summary>
    /// Description.
    /// </summary>
    public string Description { get; init; } = order.Description;

    /// <summary>
    /// Status.
    /// </summary>
    public OrderStatus Status { get; init; } = order.Status;

    /// <summary>
    /// Created date.
    /// </summary>
    public DateTime CreatedAt { get; init; } = order.CreatedAt;

    /// <summary>
    /// Las update date.
    /// </summary>
    public DateTime? UpdatedAt { get; init; } = order.UpdatedAt;
}
