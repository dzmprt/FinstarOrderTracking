using FOT.Domain.Orders;
using MitMediator;

namespace FOT.Application.UseCase.Orders.Commands.CreateOrder;

/// <summary>
/// Create order command.
/// </summary>
public struct CreateOrderCommand : IRequest<OrderDto>
{
    /// <summary>
    /// Order description.
    /// </summary>
    public string Description { get; init; }
}