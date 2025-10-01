using FOT.Domain.Orders;
using MitMediator;

namespace FOT.Application.UseCase.Orders.Queries.GetOrder;

/// <summary>
/// Get order.
/// </summary>
public class GetOrderQuery : IRequest<OrderDto>
{
    /// <summary>
    /// Order number.
    /// </summary>
    public Guid OrderNumber { get; init; }
}