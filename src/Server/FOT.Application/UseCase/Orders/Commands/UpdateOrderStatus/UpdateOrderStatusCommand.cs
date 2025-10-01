using FOT.Domain.Orders;
using FOT.Domain.Orders.Enums;
using MitMediator;

namespace FOT.Application.UseCase.Orders.Commands.UpdateOrderStatus;

/// <summary>
/// Update order status command.
/// </summary>
public struct UpdateOrderStatusCommand : IRequest<OrderDto>
{
    /// <summary>
    /// Order number.
    /// </summary>
    public Guid OrderNumber { get; private set; }

    public void SetOrderNumber(Guid orderNumber) => OrderNumber = orderNumber;
    
    /// <summary>
    /// New order status.
    /// </summary>
    public OrderStatusEnum NewStatus { get; init; }
}