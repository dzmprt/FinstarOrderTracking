using FOT.Application.Abstractions;
using FOT.Domain.Orders;
using MitMediator;

namespace FOT.Application.UseCase.Orders.Queries.GetOrder;

/// <summary>
/// Handler for <see cref="GetOrderQuery"/>.
/// </summary>
/// <param name="ordersProvider">Orders provider.</param>
public class GetOrderQueryHandler(IBaseProvider<Order> ordersProvider) : IRequestHandler<GetOrderQuery, OrderDto>
{
    /// <inheritdoc/>
    public async ValueTask<OrderDto> HandleAsync(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await ordersProvider.FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, cancellationToken);
        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderNumber);
        }

        return new OrderDto(order);
    }
}