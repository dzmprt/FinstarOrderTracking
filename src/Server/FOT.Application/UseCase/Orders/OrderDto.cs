using System.ComponentModel.DataAnnotations;
using FOT.Domain.Orders;
using FOT.Domain.Orders.Enums;

namespace FOT.Application.UseCase.Orders;

public class OrderDto
{
    /// <summary>
    /// Order number.
    /// </summary>
    public Guid OrderNumber { get; set; }
    
    /// <summary>
    /// Description.
    /// </summary>
    public string Description { get; init; }
    
    /// <summary>
    /// Status.
    /// </summary>
    public OrderStatusEnum Status { get; init; }
    
    /// <summary>
    /// Created date.
    /// </summary>
    public DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// Las update date.
    /// </summary>
    public DateTime? UpdatedAt { get; init; }

    public OrderDto(Order order)
    {
        OrderNumber = order.OrderNumber;
        Description = order.Description;
        Status = order.Status;
        CreatedAt = order.CreatedAt;
        UpdatedAt = order.UpdatedAt;
    }
}